/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CMSPresenter;
using UtilityExtensions;
using System.Web.Mvc;
using CmsData;

namespace CmsWeb.Models
{
	public class ContributionSearchModel : PagerModel2
	{
		public string Name { get; set; }
		public string Comments { get; set; }
		public int? Type { get; set; }
		public int? Status { get; set; }
		private int? _peopleId;
		public int? PeopleId
		{
			get { return _peopleId; }
			set
			{
				_peopleId = value;
				if (value != null) person = DbUtil.Db.LoadPersonById(value.Value);
			}
		}

		public int Year { get; set; }
		public int FundId { get; set; }
		public Person person { get; set; }

		public ContributionSearchModel()
		{
			GetCount = Count;
			Sort = "Date";
			Direction = "desc";
		}

		private IQueryable<Contribution> contributions;
		public IEnumerable<ContributionInfo> ContributionsList()
		{
			contributions = FetchContributions();
			if (!_count.HasValue)
				_count = contributions.Count();
			contributions = ApplySort(contributions).Skip(StartRow).Take(PageSize);
			return ContributionsList(contributions);
		}

		private IEnumerable<ContributionInfo> ContributionsList(IQueryable<Contribution> query)
		{
			var q2 = from c in query
					 let bd = c.BundleDetails.FirstOrDefault()
					 select new ContributionInfo
					 {
						 BundleId = bd == null ? 0 : bd.BundleHeaderId,
						 ContributionAmount = c.ContributionAmount,
						 ContributionDate = c.ContributionDate,
						 ContributionId = c.ContributionId,
						 ContributionType = c.ContributionType.Description,
						 ContributionTypeId = c.ContributionTypeId,
						 Fund = c.ContributionFund.FundName,
						 Pledge = c.PledgeFlag,
						 StatusId = c.ContributionStatusId,
						 Status = c.ContributionStatus.Description,
						 Name = c.Person.Name,
						 PeopleId = c.PeopleId,
						 Description = c.ContributionDesc,
						 CheckNo = c.CheckNo
					 };
			return q2;
		}

		private int? _count;
		public int Count()
		{
			if (!_count.HasValue)
				_count = FetchContributions().Count();
			return _count.Value;
		}
		private IQueryable<Contribution> FetchContributions()
		{
			if (contributions != null)
				return contributions;

			contributions = from c in DbUtil.Db.Contributions
							where c.PeopleId == PeopleId || PeopleId == null
							select c;

			if (Name.HasValue())
				contributions = from c in contributions
								where c.Person.Name.Contains(Name)
								select c;

			if (Comments.HasValue())
				contributions = from c in contributions
								where c.ContributionDesc.Contains(Comments)
									|| c.CheckNo == Comments
								select c;

			if ((Type ?? 0) != 0)
				contributions = from c in contributions
								where c.ContributionTypeId == Type
								select c;

			if ((Status ?? 99) != 99)
				contributions = from c in contributions
								where c.ContributionStatusId == Status
								select c;

			if (Year > 0)
				contributions = from c in contributions
								where c.ContributionDate.Value.Year == Year
								select c;

			if (FundId > 0)
				contributions = from c in contributions
								where c.FundId == FundId
								select c;

			return contributions;
		}
		public IQueryable<Contribution> ApplySort(IQueryable<Contribution> q)
		{
			if ((Direction ?? "desc") == "asc")
				switch (Sort)
				{
					case "Date":
						q = q.OrderBy(c => c.ContributionDate);
						break;
					case "Amount":
						q = from c in q
							orderby c.ContributionAmount, c.ContributionDate descending
							select c;
						break;
					case "Type":
						q = from c in q
							orderby c.ContributionTypeId, c.ContributionDate descending
							select c;
						break;
					case "Pledge":
						q = from c in q
							orderby c.PledgeFlag, c.ContributionDate descending
							select c;
						break;
					case "Status":
						q = from c in q
							orderby c.ContributionStatusId, c.ContributionDate descending
							select c;
						break;
					case "Fund":
						q = from c in q
							orderby c.FundId, c.ContributionDate descending
							select c;
						break;
				}
			else
				switch (Sort)
				{
					case "Date":
						q = q.OrderByDescending(c => c.ContributionDate);
						break;
					case "Amount":
						q = from c in q
							orderby c.ContributionAmount descending, c.ContributionDate descending
							select c;
						break;
					case "Type":
						q = from c in q
							orderby c.ContributionTypeId descending, c.ContributionDate descending
							select c;
						break;
					case "Pledge":
						q = from c in q
							orderby c.PledgeFlag descending, c.ContributionDate descending
							select c;
						break;
					case "Status":
						q = from c in q
							orderby c.ContributionStatusId descending, c.ContributionDate descending
							select c;
						break;
					case "Fund":
						q = from c in q
							orderby c.FundId descending, c.ContributionDate descending
							select c;
						break;
				}
			return q;
		}
		public SelectList ContributionStatuses()
		{
			return new SelectList(new CodeValueController().ContributionStatuses99(),
				"Id", "Value", Status.ToString());
		}
		public SelectList ContributionTypes()
		{
			return new SelectList(new CodeValueController().ContributionTypes0(),
				"Id", "Value", Type.ToString());
		}
		public IEnumerable<SelectListItem> Years()
		{
			var q = from c in DbUtil.Db.Contributions
					where c.PeopleId == PeopleId || PeopleId == null
					group c by c.ContributionDate.Value.Year
						into g
						orderby g.Key descending
						select new SelectListItem { Text = g.Key.ToString() };
			var list = q.ToList();
			list.Insert(0, new SelectListItem { Text = "(not specified)", Value = "0" });
			return list;
		}
        public decimal Total()
        {
        	var q = FetchContributions();
			q = from c in q
				where c.ContributionStatusId == (int)Contribution.StatusCode.Recorded
                where !ReturnedReversedTypes.Contains(c.ContributionTypeId)
                where c.PledgeFlag == false
                select c;
            var t = q.Sum(c => c.ContributionAmount);
            if (t.HasValue)
                return t.Value;
            return 0;
        }

		public IEnumerable<SelectListItem> Funds()
		{
			var list = (from c in DbUtil.Db.Contributions
						where c.PeopleId == PeopleId || PeopleId == null
						group c by new { c.FundId, c.ContributionFund.FundName }
							into g
							orderby g.Key.FundName
							select new SelectListItem()
								   {
									   Value = g.Key.FundId.ToString(),
									   Text = g.Key.FundName
								   }).ToList();
			list.Insert(0, new SelectListItem { Text = "(not specified)", Value = "0" });
			return list;
		}        public static int[] ReturnedReversedTypes = new int[] 
        { 
            (int)Contribution.TypeCode.ReturnedCheck, 
            (int)Contribution.TypeCode.Reversed 
        };
		public class ContributionInfo
		{
			public int BundleId { get; set; }
			public int ContributionId { get; set; }
			public string Fund { get; set; }
			public string ContributionType { get; set; }
			public int? ContributionTypeId { get; set; }
			public int? PeopleId { get; set; }
			public string Name { get; set; }
			public DateTime? ContributionDate { get; set; }
			public decimal? ContributionAmount { get; set; }
			public string Status { get; set; }
			public int? StatusId { get; set; }
			public bool Pledge { get; set; }
			public string Description { get; set; }
			public string CheckNo { get; set; }
		}
	}
}

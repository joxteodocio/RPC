$(function(){$(".datepicker").datepicker({dateFormat:"m/d/yy",changeMonth:!0,changeYear:!0}),$("#run").click(function(n){n.preventDefault();var i=$(this).closest("form"),t=i.serialize();$.post("/FinanceReports/TotalsByFundResults",t,function(n){$("#results").html(n).ready(function(){$("table.grid tbody tr:even").addClass("alt")})})}),$("#export").click(function(n){n.preventDefault(),$.blockUI({theme:!0,title:"Producing Contributions Export",message:"<p>Click the page to continue after your download appears.</p>"});var i=$(this).closest("form"),t=i.serialize();window.location="/Export/Contributions?totals=false&"+t,$(".blockOverlay").attr("title","Click to unblock").click($.unblockUI)}),$("#exporttotals").click(function(n){n.preventDefault(),$.blockUI({theme:!0,title:"Producing Contribution Totals Export",message:"<p>Click the page to continue after your download appears.</p>"});var i=$(this).closest("form"),t=i.serialize();window.location="/Export/Contributions?totals=true&"+t,$(".blockOverlay").attr("title","Click to unblock").click($.unblockUI)}),$(".bt").button()})
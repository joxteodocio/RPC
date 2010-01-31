﻿<%@ Page Language="C#" MasterPageFile="~/Views/Shared/bvorg.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.LoveRespectModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
    <title>Love and Respect Small Group Registration</title>
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">

    <script src="/Content/js/jquery-1.4.1.min.js" type="text/javascript"></script>    

    <script type="text/javascript">
        $(function() {
            $("#zip1").change(function() {
                $.post('/Register/CityState/' + $(this).val(), null, function(ret) {
                    if (ret) {
                        $('#state1').val(ret.state);
                        $('#city1').val(ret.city);
                    }
                }, 'json');
            });
        });
        $(function() {
            $("#zip2").change(function() {
                $.post('/Register/CityState/' + $(this).val(), null, function(ret) {
                    if (ret) {
                        $('#state2').val(ret.state);
                        $('#city2').val(ret.city);
                    }
                }, 'json');
            });
            $('#same').click(function() {
                $('#addr2').val($('#addr1').val());
                $('#state2').val($('#state1').val());
                $('#city2').val($('#city1').val());
                $('#zip2').val($('#zip1').val());
                return false;
            });
        });
    </script>    
    <h2>Register for a Love and Respect small group</h2>

    <% using (Html.BeginForm()) { %>
    <%=Html.Hidden("shownew1", "true") %><%=Html.Hidden("shownew2", "true") %>
        <div>
            <fieldset>
                <table style="empty-cells:show">
                <col style="width: 13em; text-align:right" />
                <col />
                <col />
                <tr>
                    <td><label for="night">Preferred Night</label></td>
                    <td><%= Html.DropDownList("night", CMSWeb.Models.LoveRespectModel.Nights())%></td>
                    <td><%= Html.ValidationMessage("night") %></td>
                </tr>
                <tr>
                    <td><label for="location">Preferred church/location</label></td>
                    <td><%= Html.TextBox("location") %></td>
                    <td><%= Html.ValidationMessage("location") %></td>
                </tr>
                <tr><th colspan="2">His Info</th></tr>
                <tr>
                    <td><label for="first1">First Name</label></td>
                    <td><%= Html.TextBox("first1") %></td>
                    <td><%= Html.ValidationMessage("first1") %></td>
                </tr>
                <tr>
                    <td><label for="lastname1">Last Name</label></td>
                    <td><%= Html.TextBox("lastname1") %></td>
                    <td><%= Html.ValidationMessage("lastname1") %></td>
                </tr>
                <tr>
                    <td><label for="dob1">Date of Birth</label></td>
                    <td><%= Html.TextBox("dob1") %></td>
                    <td><%= Html.ValidationMessage("dob1") %></td>
                </tr>
                <tr>
                    <td><label for="phone1">Phone #</label></td>
                    <td><%= Html.TextBox("phone1")%></td>
                    <td><%= Html.RadioButton("homecell1", "h") %> Home
                    <%= Html.RadioButton("homecell1", "c") %> Cell
                    <%= Html.ValidationMessage("phone1")%></td>
                </tr>
                <tr>
                    <td><label for="email1">Email</label></td>
                    <td><%= Html.TextBox("email1") %></td>
                    <td><%= Html.ValidationMessage("email1") %></td>
                </tr>
                <tr>
                    <td><label for="addr1">Address</label></td>
                    <td><%=Html.TextBox("addr1") %></td>
                    <td><%= Html.ValidationMessage("addr1") %></td>
                </tr>
                <tr>
                    <td><label for="zip1">Zip</label></td>
                    <td><%=Html.TextBox("zip1")%></td>
                    <td><%= Html.ValidationMessage("zip1")%></td>
                </tr>
                <tr>
                    <td><label for="city1">City</label></td>
                    <td><%=Html.TextBox("city1")%></td>
                    <td></td>
                </tr>
                <tr>
                    <td><label for="state1">State</label></td>
                    <td><%=Html.TextBox("state1")%></td>
                    <td></td>
                </tr>
                <tr><th colspan="2">Her Info</th></tr>
                <tr>
                    <td><label for="first2">First Name</label></td>
                    <td><%= Html.TextBox("first2") %></td>
                    <td><%= Html.ValidationMessage("first2") %></td>
                </tr>
                <tr>
                    <td><label for="lastname2">Last Name</label></td>
                    <td><%= Html.TextBox("lastname2") %></td>
                    <td><%= Html.ValidationMessage("lastname2") %></td>
                </tr>
                <tr>
                    <td><label for="dob2">Date of Birth</label></td>
                    <td><%= Html.TextBox("dob2") %></td>
                    <td><%= Html.ValidationMessage("dob2") %></td>
                </tr>
                <tr>
                    <td><label for="phone2">Phone #</label></td>
                    <td><%= Html.TextBox("phone2")%></td>
                    <td><%= Html.RadioButton("homecell2", "h") %> Home
                    <%= Html.RadioButton("homecell2", "c") %> Cell
                    <%= Html.ValidationMessage("phone2")%></td>
                </tr>
                <tr>
                    <td><label for="email2">Email</label></td>
                    <td><%= Html.TextBox("email2") %></td>
                    <td><%= Html.ValidationMessage("email2") %></td>
                </tr>
                <tr>
                    <td><label for="addr2">Address <a id="same" href="#">(same)</a></label> </td>
                    <td><%=Html.TextBox("addr2") %></td>
                    <td><%= Html.ValidationMessage("addr2") %></td>
                </tr>
                <tr>
                    <td><label for="zip2">Zip</label></td>
                    <td><%=Html.TextBox("zip2")%></td>
                    <td><%= Html.ValidationMessage("zip2")%></td>
                </tr>
                <tr>
                    <td><label for="city2">City</label></td>
                    <td><%=Html.TextBox("city2")%></td>
                    <td></td>
                </tr>
                <tr>
                    <td><label for="state2">State</label></td>
                    <td><%=Html.TextBox("state2")%></td>
                    <td></td>
                </tr>
                
                <tr><th colspan="2">Relationship</th></tr>
                <tr>
                    <td><label for="Relation">Relationship</label></td>
                    <td><%=Html.DropDownList("Relation", CMSWeb.Models.LoveRespectModel.Relations())%></td>
                    <td><%= Html.ValidationMessage("Relation")%></td>
                </tr>
                
                <tr>
                    <td>&nbsp;</td><td><input type="submit" value="Submit" /></td>
                </tr>
                </table>
            </fieldset>
            <p>If you are having difficulty registering online, <%=CmsData.DbUtil.Settings("LRHelp", "change LRHelp setting") %></p>
        </div>
    <% } %>
</asp:Content>

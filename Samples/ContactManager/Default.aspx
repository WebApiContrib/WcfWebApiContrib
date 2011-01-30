<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ContactManager.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
        "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Contact Manager</title>
    <link href="main.css" rel="stylesheet" type="text/css" />
    <link href="http://ajax.microsoft.com/ajax/jquery.ui/1.8.5/themes/ui-lightness/jquery-ui.css"
        rel="stylesheet" type="text/css" />
    <script src="http://ajax.microsoft.com/ajax/jquery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="http://ajax.microsoft.com/ajax/jquery.ui/1.8.5/jquery-ui.min.js" type="text/javascript"></script>
    <script src="http://ajax.microsoft.com/ajax/jquery.templates/beta1/jquery.tmpl.js"
        type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#addContact").submit(function () {
                $.post(
                    "contacts/",
                    $("#addContact").serialize(),
                    function (value) {
                        $("#contactTemplate").tmpl(value).appendTo("#contacts");
                    }
                );
                return false;
            });
            $(".removeContact").live("click", function () {
                $.ajax({
                    type: "DELETE",
                    url: $(this).attr("href"),
                    context: this,
                    success: function () {
                        $(this).closest("li").remove();
                    }
                });
                return false;
            });
            $("body").addClass("ui-widget");
            $("#contacts li").addClass("ui-widget-content ui-corner-all");
            $("#contacts li h1").addClass("ui-widget-header");
            $("input[type=\"submit\"], .removeContact, .viewAsXml").button();

        });
    </script>
    <script id="contactTemplate" type="text/html">
            <li class="ui-widget-content ui-corner-all">
                <h1 class="ui-widget-header">${ Name }</h1>
                <p>${ Address }, ${ City } ${ State } ${ Zip }<br />
                    <a href="mailto:${ Email }">${ Email }</a><br/>
                    <a href="http://twitter.com/${ Twitter }">@${ Twitter }</a><br /><br />
                    <p><a href="${ Self }" class="viewAsXml ui-state-default ui-corner-all" target='_blank'>View as <%=this.ViewAsButtonCaption%></a></p>
                    <p><a href="${ Self }" class="removeContact ui-state-default ui-corner-all">Remove</a></p>
                </p>
            </li>
    </script>
</head>
<body>
    <ul id="contacts">
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <li>
                    <h1>
                        <%#DataBinder.Eval(Container.DataItem, "Name")%></h1>
                    <p>
                        <%#DataBinder.Eval(Container.DataItem, "Address")%>,
                        <%#DataBinder.Eval(Container.DataItem, "City")%>
                        <%#DataBinder.Eval(Container.DataItem, "State")%>
                        <%#DataBinder.Eval(Container.DataItem, "Zip")%><br />
                        <a href="mailto:<%#DataBinder.Eval(Container.DataItem, "Email")%>">
                            <%#DataBinder.Eval(Container.DataItem, "Email")%></a><br />
                        <a href="http://twitter.com/<%#DataBinder.Eval(Container.DataItem, "Twitter")%>">@<%#DataBinder.Eval(Container.DataItem, "Twitter")%></a><br /><br />
                        <a href="<%#DataBinder.Eval(Container.DataItem, "Self")%>" class="viewAsXml" target='_blank'>
                            View as <%=ViewAsButtonCaption%></a><br />
                        <p>
                            <a href="<%#DataBinder.Eval(Container.DataItem, "Self")%>" class="removeContact">Remove</a><br />
                        </p>
                    </p>
                </li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
    <form method="post" id="addContact">
    <fieldset>
        <legend>Add New Contact</legend>
        <ol>
            <li>
                <label for="Name">
                    Name</label>
                <input type="text" name="Name" />
            </li>
            <li>
                <label for="Address">
                    Address</label>
                <input type="text" name="Address" />
            </li>
            <li>
                <label for="City">
                    City</label>
                <input type="text" name="City" />
            </li>
            <li>
                <label for="State">
                    State</label>
                <input type="text" name="State" />
            </li>
            <li>
                <label for="Zip">
                    Zip</label>
                <input type="text" name="Zip" />
            </li>
            <li>
                <label for="Email">
                    E-mail</label>
                <input type="text" name="Email" />
            </li>
            <li>
                <label for="Twitter">
                    Twitter</label>
                <input type="text" name="Twitter" />
            </li>
        </ol>
        <input type="submit" value="Add" />
    </fieldset>
    </form>
</body>
</html>

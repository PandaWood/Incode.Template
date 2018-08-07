<%
if (Session["userid"] == null)
    Response.Redirect("login.asp");
%>
<html>
    <head>
        <title>TextTemplate Test Project</title>
    </head>
    <body>
        <h1>Test web-server</h1>

        <h2>Hello, <%= Session["userid"] %>!</h2>

<%
    if (QueryString["value"] != null)
    {
%>
        <b>Current value: <%= QueryString["value"] %></b>
<%
    }
%>
        <form method="get" action="index.asp">
            <b>New value:</b> <input type="text" name="value" value="<%= QueryString["value"] %>" /><br />
            <input type="submit" />
        </form>

        <a href="logout.asp">Log out!</a>
    </body>
</html>
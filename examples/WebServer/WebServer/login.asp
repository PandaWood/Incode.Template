<%
    if (QueryString["u"] != null && QueryString["p"] == "password")
    {
        Session["userid"] = QueryString["u"];
        Response.Redirect("index.asp");
    }
%>
<html>
    <head>
        <title>TextTemplate Test Project</title>
    </head>
    <body>
        <h1>Login-form</h1>

        <form method="get" action="login.asp">
            Username: <input type="text" name="u" /><br />
            Password: <input type="password" name="p" /> (hint: use "password", without the quotes)<br />
            <input type="submit" />
        </form>
    </body>
</html>
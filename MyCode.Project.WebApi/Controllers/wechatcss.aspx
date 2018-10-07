@charset "utf-8";
<%@ Page Language="C#"  %>
<%@ Import Namespace="Wolf.Project.Infrastructure.UnityExtensions" %>
<%@ Import Namespace="Wolf.Project.Services" %>
<%@ Import Namespace="Wolf.Project.Domain.Message.Act" %>
<%@ Import Namespace="Wolf.Project.Infrastructure.Constant" %>
<% 
    var css = @".primary-color {
    color: @primary-color;
}
.title-color {
    color: @title-color;
}
.title-border-color {
    border-color: @title-color;
}
.text-color {
    color: @text-color;
}
.text-border-color {
    color: @text-color;
}
.disable-color {
    color: @disable-color;
}
.disable-bg-color {
    background-color: @disable-color;
}
.disable-border-color {
    border-color: @disable-color;
}
.success-color {
    color: @success-color;
}
.success-bg-color {
    background-color: @success-color;
}
.warning-color {
    color: @warning-color;
}
.warning-bg-color {
    background-color: @warning-color;
}
.primary-bg-color {
    background-color: @primary-color;
}
.primary-border-color {
    border-color: @primary-color;
}
.pg-btn.disabled {
    background: @disable-bg-color;
}
.gesturepsw .patt-circ {
    border-color: @primary-color;
 }
.gesturepsw .patt-circ.hovered .patt-big-dots {
    background: @primary-color;
}
.gesturepsw .patt-error .patt-circ.hovered .patt-big-dots {
    background: @warning-color;
}
.gesturepsw .patt-success .patt-circ.hovered .patt-big-dots {
    background: @success-color;
}
.gesturepsw .patt-error .patt-circ.hovered .patt-dots {
    background: @warning-color;
}
.gesturepsw .patt-success .patt-circ.hovered .patt-dots {
    background: @success-color;
}
.gesturepsw .patt-dots {
    background: @primary-color;
}
.gesturepsw .patt-circ.hovered .patt-dots {
    background: @primary-color;
}
.gesturepsw .patt-success .patt-lines {
    background: @success-color;
}
.gesturepsw .patt-error .patt-lines {
    background: @warning-color;
}
.gesturepsw .patt-lines {
    background: @primary-color;
}";
    var domain = Request.QueryString["domain"];

    var basicService = UnityHelper.GetService<IBasicService>();

    var list = basicService.GetConfigData<List<ConfigColorItem>>(domain, SysGlobalCodeConst.WechatColorConifgCode);

    foreach (var item in list) {

        css = css.Replace(item.StyleName,item.Color);

    }

    Response.Write(css);
%>
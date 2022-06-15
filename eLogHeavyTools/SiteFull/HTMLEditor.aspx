<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HTMLEditor.aspx.cs" Inherits="Site.HTMLEditor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><% Response.Write(eProjectWeb.Framework.ModuleRegistry.AppTitle); %></title>

    <meta charset="utf-8" />
    <!-- Viewport mobile tag for sensible mobile support -->
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta http-equiv="x-ua-compatible" content="IE=edge" />
    <meta http-equiv="Cache-Control" content="no-cache, no-store, must-revalidate" />
    <meta http-equiv="Pragma" content="no-cache" />
    <meta http-equiv="Expires" content="0" />

    <link rel="SHORTCUT ICON" href="favicon.ico" />

    <!-- include libraries(jQuery, bootstrap) -->
    <link rel="stylesheet" href="Styles/vendor/bootstrap-3.3.5.min.css" />
    <!--<!-- include summernote css/js-->
    <link rel="stylesheet" href="Styles/vendor/summernote-0.8.1.min.css" />

</head>
<body>
    <div id="summernote"></div>

    <!-- include libraries(jQuery, bootstrap) -->
    <script src="Scripts/vendor/jQuery-3.1.0.min.js"></script>
    <script src="Scripts/vendor/SummerNote/bootstrap-3.3.5.min.js"></script>
    <!--<!-- include summernote css/js-->
    <script src="Scripts/vendor/SummerNote/summernote-0.8.1.min.js"></script>
    <!-- include summernote-hu-HU -->
    <%--<script src="dist/lang/summernote-hu-HU.js"></script>--%>

    <script>
        $(document).ready(function () {

            window["initSummerNote"] = function (option) {

                var deferred = $.Deferred(), promise = deferred.promise(),
                    self = this;

                var _init = function () {
                    var $summerNote = $("#summernote");
                    var summerNote = $summerNote
                        .summernote(option)
                        .data("summernote");
                    summerNote.disable();
                    $summerNote.summernote("fullscreen.toggle");
                    deferred.resolveWith(self, [summerNote, $summerNote]);
                };

                if ("lang" in option && option["lang"] !== "en-US") {
                    var script = "Scripts/vendor/SummerNote/lang/summernote-" + option["lang"] + ".min.js";
                    $.getScript(script, function () {
                        _init();
                    });
                } else {
                    _init();
                }

                return promise;
            };

        });
    </script>
</body>
</html>

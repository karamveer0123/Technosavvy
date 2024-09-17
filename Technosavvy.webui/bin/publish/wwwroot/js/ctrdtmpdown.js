
$(function () {


    function addDownloadScreen() {
        var thm = $('#themeChange').attr('href')
        if (thm == '/css/dark-theme-root.css') {
            $('#Q1').css({ 'background': 'rgba(20,19,19,1)' });
        }
    }

    $(".downloadScreen").click(function () {

        addDownloadScreen()
        let Canv_Widget = $(this).attr("data-id");
        html2canvas($("#" + Canv_Widget), {
            onrendered: function (canvas) {
                saveAs(canvas.toDataURL(), "screensort.png");
            }
        });

        var Q1Style = setInterval(function () {
            $('#Q1').removeAttr('style');
        }, 2000)

        clearInterval(function () {
            Q1Style
        }, 2000)

    });

    function saveAs(uri, filename) {
        var link = document.createElement("a");
        if (typeof link.download === "string") {
            link.href = uri;
            link.download = filename;
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        } else {
            window.open(uri);
        }
    }
});
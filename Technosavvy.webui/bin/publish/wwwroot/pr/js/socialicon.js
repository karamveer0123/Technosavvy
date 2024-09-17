setShareLinks();

function socialWindowScreen(url) {

    var left = (screen.width - 570)/2;
    var top = (screen.height - 570)/2;

    var params = "menubar=no,toolbar=no,status=no,width=570,height=570,top="+top + ",left=" + left;

    window.open(url,"NewWindow",params);
}

//
function  setShareLinks() {


    var pageUrl = encodeURIComponent(document.URL);
    var tweet = encodeURIComponent($("meta[property='og:description']").attr("content"));


    $(".social-share-url.facebook").on("click",function () {
        url = "https://www.facebook.com/sharer/sharer.php?u=" +pageUrl + "&text=Join me to earn free $3 worth of TechnoSavvy on TechnoApp Create your free account now to secure you TechnoSavvy. Referral Link:";
        socialWindowScreen(url);
    })
    $(".social-share-url.twitter").on("click",function () {
        url = "https://www.twitter.com/intent/tweet?url=" +pageUrl + "&text=Join me to earn free $3 worth of TechnoSavvy on TechnoApp Create your free account now to secure you TechnoSavvy. Referral Link:" +tweet;
        socialWindowScreen(url);
    })
    $(".social-share-url.telegram").on("click",function () {
        url = "https://t.me/share/url?url=" +pageUrl + "&text=Join me to earn free $3 worth of TechnoSavvy on TechnoApp Create your free account now to secure you TechnoSavvy. Referral Link:";
        socialWindowScreen(url);
    })
    $(".social-share-url.whatsapp").on("click",function () {
        url = "https://wa.me/?text=" +pageUrl + "&text=Join me to earn free $3 worth of TechnoSavvy on TechnoApp Create your free account now to secure you TechnoSavvy. Referral Link:";
        socialWindowScreen(url);
    })
}
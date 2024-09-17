
// Media Library Page and top 10 reason blog page Javascript

setShareLinks();
function socialWindowScreen(url) {
    var left = (screen.width - 570)/2;
    var top = (screen.height - 570)/2;
    var params = "menubar=no,toolbar=no,status=no,width=570,height=570,top="+top + ",left=" + left;
    window.open(url,"NewWindow",params);
}
 
function  setShareLinks() {
    var pageUrl = encodeURIComponent(document.URL);
    var tweet = encodeURIComponent($("meta[property='og:description']").attr("content"));

    $(".social-share-url.facebook").on("click",function () {
     url = "https://www.facebook.com/sharer/sharer.php?u=" +pageUrl + "&text=Top 10 Reasons Why you Should Invest in TechnoSavvy token in 2022";
     socialWindowScreen(url);
    })
    $(".social-share-url.twitter").on("click",function () {
     url = "https://www.twitter.com/intent/tweet?url=" +pageUrl + "&text=Top 10 Reasons Why you Should Invest in TechnoSavvy token in 2022" +tweet;
     socialWindowScreen(url);
    })
   // $(".social-share-url.linkedin").on("click",function () {
   //   url = "https://www.linkedin.com/sharing/share-offsite/?url=" +pageUrl + "&text=Top 10 Reasons Why you Should Invest in TechnoSavvy token in 2022";
   //   socialWindowScreen(url);
   // })
    $(".social-share-url.telegram").on("click",function () {
     url = "https://t.me/share/url?url=" +pageUrl + "&text=Top 10 Reasons Why you Should Invest in TechnoSavvy token in 2022";
     socialWindowScreen(url);
    })
}

// 2022 Best Altcoin blog page javascript

setShareLinks2();
function socialWindowScreen(url) {
    var left = (screen.width - 570)/2;
    var top = (screen.height - 570)/2;
    var params = "menubar=no,toolbar=no,status=no,width=570,height=570,top="+top + ",left=" + left;
    window.open(url,"NewWindow",params);
}
 
function  setShareLinks2() {
    var pageUrl = encodeURIComponent(document.URL);
    var tweet = encodeURIComponent($("meta[property='og:description']").attr("content"));

    $(".social-share-url2.facebook").on("click",function () {
     url = "https://www.facebook.com/sharer/sharer.php?u=" +pageUrl + "&text=2022 Best Altcoin: Ground-Breaking Utility Makes TechnoSavvy an Inflationary Token";
     socialWindowScreen(url);
    })
    $(".social-share-url2.twitter").on("click",function () {
     url = "https://www.twitter.com/intent/tweet?url=" +pageUrl + "&text=2022 Best Altcoin: Ground-Breaking Utility Makes TechnoSavvy an Inflationary Token" +tweet;
     socialWindowScreen(url);
    })
    $(".social-share-url2.telegram").on("click",function () {
     url = "https://t.me/share/url?url=" +pageUrl + "&text=2022 Best Altcoin: Ground-Breaking Utility Makes TechnoSavvy an Inflationary Token";
     socialWindowScreen(url);
    })
}


// 3 Reasons to Invest bolg page Javascript

setShareLinks3();
function socialWindowScreen(url) {
    var left = (screen.width - 570)/2;
    var top = (screen.height - 570)/2;
    var params = "menubar=no,toolbar=no,status=no,width=570,height=570,top="+top + ",left=" + left;
    window.open(url,"NewWindow",params);
}
 
function  setShareLinks3() {
    var pageUrl = encodeURIComponent(document.URL);
    var tweet = encodeURIComponent($("meta[property='og:description']").attr("content"));

    $(".social-share-url3.facebook").on("click",function () {
     url = "https://www.facebook.com/sharer/sharer.php?u=" +pageUrl + "&text=3 Reasons to Invest in Early Crypto Projects";
     socialWindowScreen(url);
    })
    $(".social-share-url3.twitter").on("click",function () {
     url = "https://www.twitter.com/intent/tweet?url=" +pageUrl + "&text=3 Reasons to Invest in Early Crypto Projects" +tweet;
     socialWindowScreen(url);
    })
    $(".social-share-url3.telegram").on("click",function () {
     url = "https://t.me/share/url?url=" +pageUrl + "&text=3 Reasons to Invest in Early Crypto Projects";
     socialWindowScreen(url);
    })
}


// An Introduction to TechnoApp blog page javascript

setShareLinks4();
function socialWindowScreen(url) {
    var left = (screen.width - 570)/2;
    var top = (screen.height - 570)/2;
    var params = "menubar=no,toolbar=no,status=no,width=570,height=570,top="+top + ",left=" + left;
    window.open(url,"NewWindow",params);
}
 
function  setShareLinks4() {
    var pageUrl = encodeURIComponent(document.URL);
    var tweet = encodeURIComponent($("meta[property='og:description']").attr("content"));

    $(".social-share-url4.facebook").on("click",function () {
     url = "https://www.facebook.com/sharer/sharer.php?u=" +pageUrl + "&text=An Introduction to TechnoApp Trading Ecosystem and TechnoSavvy Token";
     socialWindowScreen(url);
    })
    $(".social-share-url4.twitter").on("click",function () {
     url = "https://www.twitter.com/intent/tweet?url=" +pageUrl + "&text=An Introduction to TechnoApp Trading Ecosystem and TechnoSavvy Token" +tweet;
     socialWindowScreen(url);
    })
    $(".social-share-url4.telegram").on("click",function () {
     url = "https://t.me/share/url?url=" +pageUrl + "&text=An Introduction to TechnoApp Trading Ecosystem and TechnoSavvy Token";
     socialWindowScreen(url);
    })
}


// Why Should You Invest blog page css

setShareLinks5();
function socialWindowScreen(url) {
    var left = (screen.width - 570)/2;
    var top = (screen.height - 570)/2;
    var params = "menubar=no,toolbar=no,status=no,width=570,height=570,top="+top + ",left=" + left;
    window.open(url,"NewWindow",params);
}
 
function  setShareLinks5() {
    var pageUrl = encodeURIComponent(document.URL);
    var tweet = encodeURIComponent($("meta[property='og:description']").attr("content"));

    $(".social-share-url5.facebook").on("click",function () {
     url = "https://www.facebook.com/sharer/sharer.php?u=" +pageUrl + "&text=Why Should You Invest in Crypto Winter?";
     socialWindowScreen(url);
    })
    $(".social-share-url5.twitter").on("click",function () {
     url = "https://www.twitter.com/intent/tweet?url=" +pageUrl + "&text=Why Should You Invest in Crypto Winter?" +tweet;
     socialWindowScreen(url);
    })
    $(".social-share-url5.telegram").on("click",function () {
     url = "https://t.me/share/url?url=" +pageUrl + "&text=Why Should You Invest in Crypto Winter?";
     socialWindowScreen(url);
    })
}


// Earn High Returns blog page javascript

setShareLinks6();
function socialWindowScreen(url) {
    var left = (screen.width - 570)/2;
    var top = (screen.height - 570)/2;
    var params = "menubar=no,toolbar=no,status=no,width=570,height=570,top="+top + ",left=" + left;
    window.open(url,"NewWindow",params);
}
 
function  setShareLinks6() {
    var pageUrl = encodeURIComponent(document.URL);
    var tweet = encodeURIComponent($("meta[property='og:description']").attr("content"));

    $(".social-share-url6.facebook").on("click",function () {
     url = "https://www.facebook.com/sharer/sharer.php?u=" +pageUrl + "&text=Earn High Returns by Staking Crypto: TechnoSavvy Staking Benefit";
     socialWindowScreen(url);
    })
    $(".social-share-url6.twitter").on("click",function () {
     url = "https://www.twitter.com/intent/tweet?url=" +pageUrl + "&text=Earn High Returns by Staking Crypto: TechnoSavvy Staking Benefit" +tweet;
     socialWindowScreen(url);
    })
    $(".social-share-url6.telegram").on("click",function () {
     url = "https://t.me/share/url?url=" +pageUrl + "&text=Earn High Returns by Staking Crypto: TechnoSavvy Staking Benefit";
     socialWindowScreen(url);
    })
}


// The new trends blog page javacrit

setShareLinks7();
function socialWindowScreen(url) {
    var left = (screen.width - 570)/2;
    var top = (screen.height - 570)/2;
    var params = "menubar=no,toolbar=no,status=no,width=570,height=570,top="+top + ",left=" + left;
    window.open(url,"NewWindow",params);
}
 
function  setShareLinks7() {
    var pageUrl = encodeURIComponent(document.URL);
    var tweet = encodeURIComponent($("meta[property='og:description']").attr("content"));

    $(".social-share-url7.facebook").on("click",function () {
     url = "https://www.facebook.com/sharer/sharer.php?u=" +pageUrl + "&text=The new trends in crypto and blockchain technology";
     socialWindowScreen(url);
    })
    $(".social-share-url7.twitter").on("click",function () {
     url = "https://www.twitter.com/intent/tweet?url=" +pageUrl + "&text=The new trends in crypto and blockchain technology" +tweet;
     socialWindowScreen(url);
    })
    $(".social-share-url7.telegram").on("click",function () {
     url = "https://t.me/share/url?url=" +pageUrl + "&text=The new trends in crypto and blockchain technology";
     socialWindowScreen(url);
    })
}


// Things to Consider blog page javascript

setShareLinks8();
function socialWindowScreen(url) {
    var left = (screen.width - 570)/2;
    var top = (screen.height - 570)/2;
    var params = "menubar=no,toolbar=no,status=no,width=570,height=570,top="+top + ",left=" + left;
    window.open(url,"NewWindow",params);
}
 
function  setShareLinks8() {
    var pageUrl = encodeURIComponent(document.URL);
    var tweet = encodeURIComponent($("meta[property='og:description']").attr("content"));

    $(".social-share-url8.facebook").on("click",function () {
     url = "https://www.facebook.com/sharer/sharer.php?u=" +pageUrl + "&text=Things to Consider Before Investing in Crypto";
     socialWindowScreen(url);
    })
    $(".social-share-url8.twitter").on("click",function () {
     url = "https://www.twitter.com/intent/tweet?url=" +pageUrl + "&text=Things to Consider Before Investing in Crypto" +tweet;
     socialWindowScreen(url);
    })
    $(".social-share-url8.telegram").on("click",function () {
     url = "https://t.me/share/url?url=" +pageUrl + "&text=Things to Consider Before Investing in Crypto";
     socialWindowScreen(url);
    })
}


// What is Value blog page javascript

setShareLinks9();
function socialWindowScreen(url) {
    var left = (screen.width - 570)/2;
    var top = (screen.height - 570)/2;
    var params = "menubar=no,toolbar=no,status=no,width=570,height=570,top="+top + ",left=" + left;
    window.open(url,"NewWindow",params);
}
 
function  setShareLinks9() {
    var pageUrl = encodeURIComponent(document.URL);
    var tweet = encodeURIComponent($("meta[property='og:description']").attr("content"));

    $(".social-share-url9.facebook").on("click",function () {
     url = "https://www.facebook.com/sharer/sharer.php?u=" +pageUrl + "&text=What is Value Variance Inflationary Token?";
     socialWindowScreen(url);
    })
    $(".social-share-url9.twitter").on("click",function () {
     url = "https://www.twitter.com/intent/tweet?url=" +pageUrl + "&text=What is Value Variance Inflationary Token?" +tweet;
     socialWindowScreen(url);
    })
    $(".social-share-url9.telegram").on("click",function () {
     url = "https://t.me/share/url?url=" +pageUrl + "&text=What is Value Variance Inflationary Token?";
     socialWindowScreen(url);
    })
}


// What Makes TechnoSavvy A Gold blog page js

setShareLinks10();
function socialWindowScreen(url) {
    var left = (screen.width - 570)/2;
    var top = (screen.height - 570)/2;
    var params = "menubar=no,toolbar=no,status=no,width=570,height=570,top="+top + ",left=" + left;
    window.open(url,"NewWindow",params);
}
 
function  setShareLinks10() {
    var pageUrl = encodeURIComponent(document.URL);
    var tweet = encodeURIComponent($("meta[property='og:description']").attr("content"));

    $(".social-share-url10.facebook").on("click",function () {
     url = "https://www.facebook.com/sharer/sharer.php?u=" +pageUrl + "&text=What Makes TechnoSavvy A Gold Standard Coin?";
     socialWindowScreen(url);
    })
    $(".social-share-url10.twitter").on("click",function () {
     url = "https://www.twitter.com/intent/tweet?url=" +pageUrl + "&text=What Makes TechnoSavvy A Gold Standard Coin?" +tweet;
     socialWindowScreen(url);
    })
    $(".social-share-url10.telegram").on("click",function () {
     url = "https://t.me/share/url?url=" +pageUrl + "&text=What Makes TechnoSavvy A Gold Standard Coin?";
     socialWindowScreen(url);
    })
}


// Encouraging Gen Z Investors blog page javascript

setShareLinks11();
function socialWindowScreen(url) {
    var left = (screen.width - 570)/2;
    var top = (screen.height - 570)/2;
    var params = "menubar=no,toolbar=no,status=no,width=570,height=570,top="+top + ",left=" + left;
    window.open(url,"NewWindow",params);
}
 
function  setShareLinks11() {
    var pageUrl = encodeURIComponent(document.URL);
    var tweet = encodeURIComponent($("meta[property='og:description']").attr("content"));

    $(".social-share-url11.facebook").on("click",function () {
     url = "https://www.facebook.com/sharer/sharer.php?u=" +pageUrl + "&text=Encouraging Gen Z Investors to Trade More and Earn More";
     socialWindowScreen(url);
    })
    $(".social-share-url11.twitter").on("click",function () {
     url = "https://www.twitter.com/intent/tweet?url=" +pageUrl + "&text=Encouraging Gen Z Investors to Trade More and Earn More" +tweet;
     socialWindowScreen(url);
    })
    $(".social-share-url11.telegram").on("click",function () {
     url = "https://t.me/share/url?url=" +pageUrl + "&text=Encouraging Gen Z Investors to Trade More and Earn More";
     socialWindowScreen(url);
    })
}


// Is it Safe to Trade blog page javascript

setShareLinks12();
function socialWindowScreen(url) {
    var left = (screen.width - 570)/2;
    var top = (screen.height - 570)/2;
    var params = "menubar=no,toolbar=no,status=no,width=570,height=570,top="+top + ",left=" + left;
    window.open(url,"NewWindow",params);
}
 
function  setShareLinks12() {
    var pageUrl = encodeURIComponent(document.URL);
    var tweet = encodeURIComponent($("meta[property='og:description']").attr("content"));

    $(".social-share-url12.facebook").on("click",function () {
     url = "https://www.facebook.com/sharer/sharer.php?u=" +pageUrl + "&text=Is it Safe to Trade Cryptocurrency?";
     socialWindowScreen(url);
    })
    $(".social-share-url12.twitter").on("click",function () {
     url = "https://www.twitter.com/intent/tweet?url=" +pageUrl + "&text=Is it Safe to Trade Cryptocurrency?" +tweet;
     socialWindowScreen(url);
    })
    $(".social-share-url12.telegram").on("click",function () {
     url = "https://t.me/share/url?url=" +pageUrl + "&text=Is it Safe to Trade Cryptocurrency?";
     socialWindowScreen(url);
    })
}


// Next-Gen Crypto blog page javascript

setShareLinks13();
function socialWindowScreen(url) {
    var left = (screen.width - 570)/2;
    var top = (screen.height - 570)/2;
    var params = "menubar=no,toolbar=no,status=no,width=570,height=570,top="+top + ",left=" + left;
    window.open(url,"NewWindow",params);
}
 
function  setShareLinks13() {
    var pageUrl = encodeURIComponent(document.URL);
    var tweet = encodeURIComponent($("meta[property='og:description']").attr("content"));

    $(".social-share-url13.facebook").on("click",function () {
     url = "https://www.facebook.com/sharer/sharer.php?u=" +pageUrl + "&text=Next-Gen Crypto Trading Experience";
     socialWindowScreen(url);
    })
    $(".social-share-url13.twitter").on("click",function () {
     url = "https://www.twitter.com/intent/tweet?url=" +pageUrl + "&text=Next-Gen Crypto Trading Experience" +tweet;
     socialWindowScreen(url);
    })
    $(".social-share-url13.telegram").on("click",function () {
     url = "https://t.me/share/url?url=" +pageUrl + "&text=Next-Gen Crypto Trading Experience";
     socialWindowScreen(url);
    })
}


// Most Rewarding Crypto blog page javascript

setShareLinks14();
function socialWindowScreen(url) {
    var left = (screen.width - 570)/2;
    var top = (screen.height - 570)/2;
    var params = "menubar=no,toolbar=no,status=no,width=570,height=570,top="+top + ",left=" + left;
    window.open(url,"NewWindow",params);
}
 
function  setShareLinks14() {
    var pageUrl = encodeURIComponent(document.URL);
    var tweet = encodeURIComponent($("meta[property='og:description']").attr("content"));

    $(".social-share-url14.facebook").on("click",function () {
     url = "https://www.facebook.com/sharer/sharer.php?u=" +pageUrl + "&text=Most Rewarding Crypto Trading Experience";
     socialWindowScreen(url);
    })
    $(".social-share-url14.twitter").on("click",function () {
     url = "https://www.twitter.com/intent/tweet?url=" +pageUrl + "&text=Most Rewarding Crypto Trading Experience" +tweet;
     socialWindowScreen(url);
    })
    $(".social-share-url14.telegram").on("click",function () {
     url = "https://t.me/share/url?url=" +pageUrl + "&text=Most Rewarding Crypto Trading Experience";
     socialWindowScreen(url);
    })
}


// Good Time to Invest blog page javascript

setShareLinks15();
function socialWindowScreen(url) {
    var left = (screen.width - 570)/2;
    var top = (screen.height - 570)/2;
    var params = "menubar=no,toolbar=no,status=no,width=570,height=570,top="+top + ",left=" + left;
    window.open(url,"NewWindow",params);
}
 
function  setShareLinks15() {
    var pageUrl = encodeURIComponent(document.URL);
    var tweet = encodeURIComponent($("meta[property='og:description']").attr("content"));

    $(".social-share-url15.facebook").on("click",function () {
     url = "https://www.facebook.com/sharer/sharer.php?u=" +pageUrl + "&text=Good Time to Invest in Crypto is Now!";
     socialWindowScreen(url);
    })
    $(".social-share-url15.twitter").on("click",function () {
     url = "https://www.twitter.com/intent/tweet?url=" +pageUrl + "&text=Good Time to Invest in Crypto is Now!" +tweet;
     socialWindowScreen(url);
    })
    $(".social-share-url15.telegram").on("click",function () {
     url = "https://t.me/share/url?url=" +pageUrl + "&text=Good Time to Invest in Crypto is Now!";
     socialWindowScreen(url);
    })
}


// Crypto Trading blog page javascript

setShareLinks16();
function socialWindowScreen(url) {
    var left = (screen.width - 570)/2;
    var top = (screen.height - 570)/2;
    var params = "menubar=no,toolbar=no,status=no,width=570,height=570,top="+top + ",left=" + left;
    window.open(url,"NewWindow",params);
}
 
function  setShareLinks16() {
    var pageUrl = encodeURIComponent(document.URL);
    var tweet = encodeURIComponent($("meta[property='og:description']").attr("content"));

    $(".social-share-url16.facebook").on("click",function () {
     url = "https://www.facebook.com/sharer/sharer.php?u=" +pageUrl + "&text=Crypto Trading is Now Rewarding";
     socialWindowScreen(url);
    })
    $(".social-share-url16.twitter").on("click",function () {
     url = "https://www.twitter.com/intent/tweet?url=" +pageUrl + "&text=Crypto Trading is Now Rewarding" +tweet;
     socialWindowScreen(url);
    })
    $(".social-share-url16.telegram").on("click",function () {
     url = "https://t.me/share/url?url=" +pageUrl + "&text=Crypto Trading is Now Rewarding";
     socialWindowScreen(url);
    })
}


// What is TechnoSavvy blog page javascript

setShareLinks17();
function socialWindowScreen(url) {
    var left = (screen.width - 570)/2;
    var top = (screen.height - 570)/2;
    var params = "menubar=no,toolbar=no,status=no,width=570,height=570,top="+top + ",left=" + left;
    window.open(url,"NewWindow",params);
}
 
function  setShareLinks17() {
    var pageUrl = encodeURIComponent(document.URL);
    var tweet = encodeURIComponent($("meta[property='og:description']").attr("content"));

    $(".social-share-url17.facebook").on("click",function () {
     url = "https://www.facebook.com/sharer/sharer.php?u=" +pageUrl + "&text=What is TechnoSavvy and Why Is It a Good Investment?";
     socialWindowScreen(url);
    })
    $(".social-share-url17.twitter").on("click",function () {
     url = "https://www.twitter.com/intent/tweet?url=" +pageUrl + "&text=What is TechnoSavvy and Why Is It a Good Investment?" +tweet;
     socialWindowScreen(url);
    })
    $(".social-share-url17.telegram").on("click",function () {
     url = "https://t.me/share/url?url=" +pageUrl + "&text=What is TechnoSavvy and Why Is It a Good Investment?";
     socialWindowScreen(url);
    })
}


// How to Earn Crypto blog page javascript

setShareLinks18();
function socialWindowScreen(url) {
    var left = (screen.width - 570)/2;
    var top = (screen.height - 570)/2;
    var params = "menubar=no,toolbar=no,status=no,width=570,height=570,top="+top + ",left=" + left;
    window.open(url,"NewWindow",params);
}
 
function  setShareLinks18() {
    var pageUrl = encodeURIComponent(document.URL);
    var tweet = encodeURIComponent($("meta[property='og:description']").attr("content"));

    $(".social-share-url18.facebook").on("click",function () {
     url = "https://www.facebook.com/sharer/sharer.php?u=" +pageUrl + "&text=How to Earn Crypto for Free?";
     socialWindowScreen(url);
    })
    $(".social-share-url18.twitter").on("click",function () {
     url = "https://www.twitter.com/intent/tweet?url=" +pageUrl + "&text=How to Earn Crypto for Free?" +tweet;
     socialWindowScreen(url);
    })
    $(".social-share-url18.telegram").on("click",function () {
     url = "https://t.me/share/url?url=" +pageUrl + "&text=How to Earn Crypto for Free?";
     socialWindowScreen(url);
    })
}


// How to Earn Crypto blog page javascript

setShareLinks19();
function socialWindowScreen(url) {
    var left = (screen.width - 570)/2;
    var top = (screen.height - 570)/2;
    var params = "menubar=no,toolbar=no,status=no,width=570,height=570,top="+top + ",left=" + left;
    window.open(url,"NewWindow",params);
}
 
function  setShareLinks19() {
    var pageUrl = encodeURIComponent(document.URL);
    var tweet = encodeURIComponent($("meta[property='og:description']").attr("content"));

    $(".social-share-url19.facebook").on("click",function () {
     url = "https://www.facebook.com/sharer/sharer.php?u=" +pageUrl + "&text=How to Earn Crypto for Free?";
     socialWindowScreen(url);
    })
    $(".social-share-url19.twitter").on("click",function () {
     url = "https://www.twitter.com/intent/tweet?url=" +pageUrl + "&text=How to Earn Crypto for Free?" +tweet;
     socialWindowScreen(url);
    })
    $(".social-share-url19.telegram").on("click",function () {
     url = "https://t.me/share/url?url=" +pageUrl + "&text=How to Earn Crypto for Free?";
     socialWindowScreen(url);
    })
}
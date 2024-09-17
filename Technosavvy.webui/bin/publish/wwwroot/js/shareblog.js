

var ogUrl = window.location.href;
document.getElementById("shareBtnface").addEventListener("click", function () {
    shareOnFacebook();  
});
function shareOnFacebook() {
    FB.init({
        appId: '2065469963818560',
        version: 'v10.0',
    });
    FB.ui({
        method: 'share',
        href: ogUrl,    
    }, function (response) { });
}

   /*
document.addEventListener("DOMContentLoaded", function () {
    const shareButton = document.getElementById("shareOnTwitterButton");  
    if (shareButton) {
      shareButton.addEventListener("click", function () {
        const postURL = window.location.href; 
        const twitterURL = `https://twitter.com/intent/tweet?url=${encodeURIComponent(postURL)}`;  
        window.open(twitterURL, "_blank", "width=550,height=420");
      });
    }
});
*/

var twitterShare = document.querySelector('#shareOnTwitterButton');

twitterShare.onclick = function (e) {
    e.preventDefault();
    var twitterWindow = window.open('https://twitter.com/share?url='+ ogUrl , 'twitter - popup', 'height = 350, width = 600');
    if (twitterWindow.focus) { twitterWindow.focus(); }
    return false;
}



function shareOnLinkedIn() {
    const shareUrl = ogUrl;     
    const linkedInShareUrl = `https://www.linkedin.com/sharing/share-offsite/?url=${encodeURIComponent(shareUrl)}`;
    window.open(linkedInShareUrl, '_blank', 'width=600,height=400');
}

// Function to open the Telegram Share Dialog
function shareOnTelegram() {
    var url = 'https://t.me/share/url?url=' + encodeURIComponent(ogUrl); 
    window.open(url, '_blank');
}


document.getElementById('shareBtnTelegram').addEventListener('click', function () {
    shareOnTelegram();
});
var refCode = $('#txtRefCode').val();
function shareOnWhatsApp() {   
    var blogURL = encodeURIComponent(refCode);   
    var message = encodeURIComponent("Check out this amazing blog post: " + blogURL);
    var whatsappLink = "https://wa.me/?text=" + message;
    window.open(whatsappLink, "_blank");
}
$("#shareBtnwhatsup").on("click", shareOnWhatsApp);
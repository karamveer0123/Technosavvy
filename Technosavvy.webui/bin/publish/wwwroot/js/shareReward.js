
//var ogUrl = $('meta[property="og:url"]').attr('content');
var ogUrl = $('#txtRefCode').val();
/*var fbButton = document.getElementById('shareBtnface');
fbButton.addEventListener('click', function () {
    window.open('https://www.facebook.com/sharer/sharer.php?u=' + ogUrl,
        'facebook-share-dialog',
        'width=800,height=600'
    );
    return false;
});*/


document.getElementById("shareBtnface").addEventListener("click", function () {
    FB.ui({
        method: 'share',
        href: ogUrl,             
    }, function (response) {        
        console.log(response);
    });
});


/*

document.getElementById("shareBtnface").addEventListener("click", function() {
            
    FB.init({
        appId: '849080016719281',
        xfbml: true,
        version: 'v13.0'
    });
    var shareContent = {
        method: 'share',
        href: ogUrl, 
                
    };
            
    FB.ui(shareContent, function(response) {
        if (response && !response.error_code) {
            console.log('Post was shared successfully!');
        } else {
            console.log('An error occurred while sharing the post.');
        }
    });
});
   

(function(d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) return;
    js = d.createElement(s); js.id = id;
    js.src = "https://connect.facebook.net/en_US/sdk.js";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));*/
   
document.addEventListener("DOMContentLoaded", function () {
    const shareButton = document.getElementById("shareOnTwitterButton");
  
    if (shareButton) {
      shareButton.addEventListener("click", function () {
        const postURL = window.location.href; // Get the URL of your blog post
  
        // Use the Twitter Web Intent to open a sharing dialog
        const twitterURL = `https://twitter.com/intent/tweet?url=${encodeURIComponent(postURL)}`;
  
        window.open(twitterURL, "_blank", "width=550,height=420");
      });
    }
  });

function shareOnLinkedIn() {
    // Replace YOUR_SHARE_URL with the actual URL you want to share
    const shareUrl = ogUrl;
 
    // LinkedIn Share API URL
    const linkedInShareUrl = `https://www.linkedin.com/sharing/share-offsite/?url=${encodeURIComponent(shareUrl)}`;
 
    // Open a new window to share on LinkedIn
    window.open(linkedInShareUrl, '_blank', 'width=600,height=400');
}

// Function to open the Telegram Share Dialog
function shareOnTelegram() {
    var url = 'https://t.me/share/url?url=' + encodeURIComponent(ogUrl); // Replace with the URL of your blog post
    window.open(url, '_blank');
}

// Attach the shareOnTelegram function to the button click event
document.getElementById('shareBtnTelegram').addEventListener('click', function () {
    shareOnTelegram();
});

function shareOnWhatsApp() {   
    var blogURL = encodeURIComponent(ogUrl);   
    var message = encodeURIComponent("Check out this amazing blog post: " + blogURL);
    var whatsappLink = "https://wa.me/?text=" + message;
    window.open(whatsappLink, "_blank");
}
$("#shareBtnwhatsup").on("click", shareOnWhatsApp);
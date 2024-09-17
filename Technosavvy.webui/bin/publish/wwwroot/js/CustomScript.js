function OnSuccess(data) {
    var result = data.split("|");
    if (result[0] == "SendOtp") {
        if (result[1] == "True") {
            $("#emailDiv").addClass("d-none");
            $("#otpDiv").removeClass("d-none");
            $("#addUser").attr("Action", "/Home/VerifyOtp");
            $("#userId").val(result[2]);
            CreateToasterMessage("Check Email", "The verification code has been sent to your email address", "success");

        } else if (result[1] == "False") {
            CreateToasterMessage("OTP", "Unable to send verification code on your email. please try again after some time.", "error");
        }
    } else if (result[0] == "VerifyOtp") {
        if (result[1] == "True") {
            $("#otpDiv").addClass("d-none");
            $("#passwordDiv").removeClass("d-none");
            $("#addUser").attr("Action", "/Home/CreatePassword");
            $("#frmBtn").text("Submit");
            CreateToasterMessage("OTP", "Your OTP is verified successfully. Please create your password", "success");
        } else if (result[1] == "False") {
            CreateToasterMessage("OTP", "Provide a vaild OTP", "error");
        }
    } else if (result[0] == "CreatePassword") {
        if (result[1] == "True") {
            CreateToasterMessage("Registration", "Account registered successfully! ", "success");
            window.location.href = "/Home/MinimumProfile"

        } else if (result[1] == "False") {
            CreateToasterMessage("Registration", "Password creation failed", "error");
        }
    }
}



function CallResend() {

    var counter = 60;
    var interval = setInterval(function () {
        counter--;
        // Display 'counter' wherever you want to display it.
        if (counter <= 0) {
            clearInterval(interval);

            $("#btnResend").attr("onclick", "ResendOtp()");
            $('#timeer').text("");
            return;
        } else {
            $('#timeer').text(counter);

        }
    }, 1000);
}

$("#ismulti").click(function () {
    var res = $("#ismulti").prop('checked');
    if (res) {
        $("#formMultiEnable").submit();
    }
});
$("#isDismulti").click(function () {
    var res = $("#isDismulti").prop('checked');
    if (!res) {
        $("#formMultiDisable").submit();
    }
});




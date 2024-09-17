var accounts;
var provider;
var signerPro;
var userAddress;
var transHash;
const TechnoSavvyTokenAdd = '0x4a47Eb576463f02A6E7Ceb28F7dE2c503778EA53';
var transhref = 'https://etherscan.io/tx/';
var networkName = '';
var userEthBalance = '';
const currOptions = [{
    label: 'ETH',
    value: 'ETH'
}, {
    label: 'USDT',
    value: 'USDT'
}];
var CurrencyName, hasMetamask, ethPrice, UsdtPrice, UsdtPrice18, TechnoSavvyCount, txtVal, txtVal18, currentTechnoSavvyPrice, currentEthPrice, currentUSDTPrice, minEthtoBuy, minUSDTtoBuy, isAllowance, isLoading, errorMessage, isDisabled, subtitle, AccountInfo;
var isConnected = typeof window !== 'undefined' ? localStorage.getItem("IsConnected") == true ? true : false : false;
var holdContract = typeof window !== "undefined" ? localStorage.getItem("holdContract") : "";
function openModal(msgtext) {
    var divHtml = '<div id="divConf" class="con-p1"> </div><h5 class="input-amt" Name="text-primary">' + msgtext + '</span></h5>';
    $('#divmsg').html(divHtml);
    $('#msgModel').modal('show');
}
function afterOpenModal() {
    subtitle.style.color = '#f00';
}
function closeModal() {
    $('#confBuyTechnoSavvy').modal('hide');
}
function openBuyModal() {
    $('#modelBuySteps').modal('show');
}
function closeBuyModal() {
    $("#btnWFW").removeClass().addClass('multisteps-form__progress-btn').addClass('js-active').removeClass('displayNone');
    $("#btnWFW").removeClass().addClass('multisteps-form__progress-btn').addClass('displayactive');
    $("#btnWA").removeClass().addClass('multisteps-form__progress-btn');
    $("#btnTrade").removeClass().addClass('multisteps-form__progress-btn');
    $("#btnCompleted").removeClass().addClass('multisteps-form__progress-btn');
    $("#divCompleted").removeClass('js-active').removeClass('displayBlock');
    $('#modelBuySteps').modal('hide');
}
function onHomePage() {
    isAllowance = false;
}
function currFlagChange(val) {
    var cFVal = val;
    CurrencyName = cFVal;
    clearForm();
    if (cFVal == 'USDT') {
        $('#imgCurr').attr('src', '');
        $('#imgCurr').attr('src', 'images/usdt.png');
    } else if (cFVal == 'ETH') {
        $('#imgCurr').attr('src', '');
        $('#imgCurr').attr('src', 'images/eth.png');
        document.getElementById("receiverCurr").value = userEthBalance;
    }
}
function clearForm() {
    $('#receiverCurr').val(0);
    $('#receiverTechnoSavvy').val(0);
}
function currChange(value) {
    console.log(value);
    var cVal = value;
    txtVal = (cVal);
    switch (CurrencyName) {
        case 'ETH':
            console.log('ETH');
            ethPrice = cVal;
            UsdtPrice = '';
            UsdtPrice18 = '';
            buyFromEth(cVal);
            $('#receiverTechnoSavvy').val(TechnoSavvyCount);
            break;
        case 'USDT':
            console.log('USDT');
            ethPrice = '';
            UsdtPrice = cVal
            if (cVal != '') {
                UsdtPrice18 = ethers.ethers.utils.parseEther(cVal);
            }
            buyFromUSDT(cVal);
            $('#receiverTechnoSavvy').val(TechnoSavvyCount);
            break;
        default:
    }
}
function nvcChange(value) {
    var toTechnoSavvy = value;
    switch (CurrencyName) {
        case 'ETH':
            console.log('TechnoSavvy Change  ETH');
            var toTechnoSavvy18 = ethers.utils.parseEther(toTechnoSavvy);
            var ethval = currentTechnoSavvyPrice * (toTechnoSavvy.toString() / 1e18) * currentEthPrice;
            console.log('toTechnoSavvy18 ' + toTechnoSavvy18 + ' toTechnoSavvy.toString() / 1e18 ' + toTechnoSavvy.toString() / 1e18);
            ethPrice = ethval;
            txtVal = ethval;
            TechnoSavvyCount = toTechnoSavvy;
            $('#receiverCurr').val(txtVal);
            $('#receiverTechnoSavvy').val(TechnoSavvyCount);
            break;
        case 'USDT':
            console.log('TechnoSavvy Change USDT');
            var usdtval = currentTechnoSavvyPrice * toTechnoSavvy / currentUSDTPrice;
            console.log('USDT PRICE' + usdtval);
            UsdtPrice = usdtval;
            if (value != '') {
                UsdtPrice18 = usdtval;
            }
            txtVal = usdtval;
            TechnoSavvyCount = toTechnoSavvy;
            $('#receiverCurr').val(txtVal);
            $('#receiverTechnoSavvy').val(TechnoSavvyCount);
            break;
        default:
    }
}
function buyFromEth(valEth) {
    if (valEth != '') {
        var valEth18 = ethers.utils.parseEther(valEth);
        var toTechnoSavvy = (valEth18 / currentEthPrice) / currentTechnoSavvyPrice;
        TechnoSavvyCount = toTechnoSavvy;
    }
}
function buyFromUSDT(valUSDT) {
    var toTechnoSavvy = valUSDT / currentTechnoSavvyPrice;
    TechnoSavvyCount = toTechnoSavvy;
}
function ExecutteStep(step) {
    if (step == 1) {
        $("#btnWFW").addClass('displayactive');
        $("#btnWA").removeClass('js-active');
        $("#btnTrade").removeClass('js-active');
        $("#btnCompleted").removeClass('js-active').addClass('displayNone');
    }
    if (step == 2) {
        $("#btnWFW").addClass('displayNone');
        $("#btnWFW").removeClass('displayactive');
        $("#btnWA").addClass('js-active');
        $("#btnTrade").addClass('displayactive');
        $("#btnCompleted").removeClass('js-active').removeClass('displayBlock');
        $("#divCompleted").removeClass('js-active').removeClass('displayBlock');
    }
    if (step == 3) {
        $("#btnWFW").addClass('js-active');
        $("#btnWA").addClass('js-active');
        $("#btnTrade").addClass('js-active');
        $("#btnCompleted").addClass('js-active').addClass('displayBlock');
        $("#btnCompleted").removeClass('displayNone');
        $("#btnTrade").removeClass('displayactive');
        $("#divCompleted").addClass('displayBlock');
    }
}
function ExecutteStepEth(step) {
    $("#btnWFW").addClass('displayNone');
    $("#btnWA").addClass('displayNone');
    if (step == 1) {
        $("#btnTrade").addClass('js-active');
        $("#divCompleted").addClass('displayNone');
        $("#btnCompleted").removeClass('js-active');
    }
    if (step == 2) {
        $("#btnTrade").addClass('displayBlock');
        $("#btnTrade").addClass('js-active');
        $("#divCompleted").addClass('displayBlock');
        $("#btnCompleted").addClass('js-active');
    }
}
async function TechnoSavvyReceivedFromUSDT() {
    if (holdContract != null) {
        try {
            const providerE = new ethers.providers.Web3Provider(window.ethereum);
            let signerProE = providerE.getSigner();
            const contractToken = new ethers.Contract(ercTokenAdd, ercTokenabi, signerProE);
            UsdtPrice18 = ethers.utils.parseEther(UsdtPrice.toString()).toString();
            var buyU = await holdContract.SellTechnoSavvyTokenWithUSDT(UsdtPrice18);
            console.log('After Receive TechnoSavvy' + buyU);
            isAllowance = false;
            setTranHash(buyU.hash);
            var parsed = UsdtPrice.toString();
            $('#spanTechnoSavvyCount').text(parsed);
            ExecutteStep(3);
            IsLoadingModel(false);
        } catch (e) {
            CreateToasterMessage("Error", 'TechnoSavvyReceivedFromUSDT ->' + e.message, "error");
            console.log('TechnoSavvyReceivedFromUSDT ->' + e.message);
            closeBuyModal();
            clearForm();
        }
    } else {
        console.log('Hold Contract not connected');
    }
}
async function setTranHash(tranID) {
    document.getElementById('linkText').innerText = tranID;
    document.getElementById('transHash').href = transhref + tranID;
}
async function receiveTechnoSavvy() {
    if (holdContract != null) {
        try {
            const providerE = new ethers.providers.Web3Provider(window.ethereum);
            let signerProE = providerE.getSigner();
            let signerAddress = await signerProE.getAddress();
            switch (CurrencyName) {
                case 'ETH':
                    var ethAmt = ethers.utils.parseEther(ethPrice);
                    var pp = await holdContract.GetTokensCountForETH(ethAmt);
                    console.log('Successfully buy from ETH. And ETH paid is ' + ethPrice);
                    console.log(' Total Count ' + pp);
                    let tx = {
                        from: signerAddress,
                        to: holdContract.address,
                        value: ethAmt
                    }
                    openBuyModal();
                    ExecutteStepEth(1);
                    var res = await signerProE.sendTransaction(tx);
                    setTranHash(res.hash);
                    var parsed = ethers.utils.formatUnits(pp.toString(), 'ether');
                    $('#spanTechnoSavvyCount').text(parsed);
                    ExecutteStepEth(2);
                    IsLoadingModel(false);
                    break;
                case 'USDT':
                    var pp = await holdContract.getTokenCountForUSDT(UsdtPrice);
                    console.log(' USDT : ' + UsdtPrice + ' Total Count :' + pp);
                    const _ercTokenAdd = ercTokenAdd;
                    console.log('ERC20 Token Address ' + _ercTokenAdd);
                    const contractToken = new ethers.Contract(_ercTokenAdd, ercTokenabi, signerProE);
                    console.log('Before approval Hold Contract Address :' + holdContract.address);
                    var isApproved = await contractToken.approve(holdContract.address, UsdtPrice18);
                    console.log('Approved Amount is :' + UsdtPrice18);
                    if (isApproved) {
                        var isExists = await CheckAllowance(UsdtPrice18);
                        if (isExists) {
                            ExecutteStep(2);
                            openBuyModal();
                            TechnoSavvyReceivedFromUSDT();
                        }
                    }
                    break;
                default:
            }
        } catch (e) {
            if (typeof e.error !== "undefined") {
                CreateToasterMessage("Error", 'receiveTechnoSavvy ->' + e.error.message, "error");
            } else {
                CreateToasterMessage("Error", 'receiveTechnoSavvy ->' + e.message, "error");
            }
            console.log('receiveTechnoSavvy ->' + e.message);
            closeBuyModal();
            clearForm();
        }
    }
}
async function buyTechnoSavvy() {
    try {
        if (holdContract != null) {
            IsLoadingModel(true);
            switch (CurrencyName) {
                case 'ETH':
                    var _txtVal18 = ethers.utils.parseEther(txtVal.toString()).toString();
                    if (parseInt(_txtVal18.toString()) < parseInt(minEthtoBuy.toString())) {
                        var _minEthtoBuy = ethers.utils.formatUnits(minEthtoBuy.toString(), 'ether');
                        var mintext = 'Minimum Required ETH is ' + _minEthtoBuy;
                        openModal(mintext);
                        break;
                    }
                    openBuyModal();
                    receiveTechnoSavvy();
                    break;
                case 'USDT':
                    console.log('USDT');
                    if (txtVal < minUSDTtoBuy) {
                        var mintext = 'Minimum Required USDT is ' + minUSDTtoBuy;
                        openModal(mintext);
                        break;
                    }
                    ExecutteStep(1);
                    openBuyModal();
                    receiveTechnoSavvy();
                    break;
                default:
            }
        } else {
            alert("your session has been expired.")
        }
    } catch (e) {
        CreateToasterMessage("Error", 'buyTechnoSavvy ->' + e.message, "error");
        console.log('buyTechnoSavvy ->' + e.message);
    }
}
async function CheckAllowance(newAllow18) {
    var istrue = false;
    try {
        var repeat = false;
        var allowancePre = await holdContract.allowanceForUSDTSender();
        for (var i = 0; i < 25; i++) {
            if (repeat)
                await new Promise(r => setTimeout(r, 3000));
            var allowance = await holdContract.allowanceForUSDTSender();
            if (allowance != newAllow18.toString()) {
                console.log('Repeate Allowance Count' + i + ' allowance ' + allowance);
                repeat = true;
            } else if (allowance == newAllow18.toString()) {
                isAllowance = true;
                istrue = true;
                var us1 = ethers.utils.formatUnits(allowance.toString(), 'ether');
                UsdtPrice = us1;
                UsdtPrice18 = allowance;
                TechnoSavvyCount = us1;
                console.log('Allowance is ' + isAllowance);
                return istrue;
            } else {
                isAllowance = false;
                istrue = false;
            }
        }
        return istrue;
    } catch (e) {
        CreateToasterMessage("Error", 'CheckAllowance ->' + e.message, "error");
        console.log('CheckAllowance ->' + e.message);
    }
}
async function getAccountDetails(acc) {
    localStorage.setItem("ConnectedAddress", acc);
    userAddress = acc;
    userEthBalance = await getBalance(acc);
    txtVal = userEthBalance;
    ethPrice = userEthBalance;
    document.getElementById("txtAccId").value = acc;
}
async function connecActWallet() {
    accounts = await ethereum.request({
        method: "eth_requestAccounts"
    });
    console.log("address is :" + accounts[0]);
    provider = new ethers.providers.Web3Provider(window.ethereum, "any");
    signerPro = provider.getSigner();
    userAddress = await signerPro.getAddress();
    localStorage.setItem("signerPro", signerPro);
    console.log("Provider Signer is :" + userAddress);
    provider.on('accountsChanged', getAccountDetails(userAddress));
    provider.on("network", (newNetwork, oldNetwork) => {
        if (newNetwork && oldNetwork) {
            window.location.reload();
        }
        console.log('New Network ' + newNetwork + ' Old Network ' + oldNetwork);
    }
    );
    localStorage.setItem("provider", provider);
    localStorage.setItem("IsConnected", "true");
    document.getElementById("txtAccId").value = userAddress;
    var network = await provider.getNetwork();
    networkName = network.name;
    document.getElementById("txtNetworkId").value = networkName;
    console.log('Network Name ' + networkName);
    isConnected = true;
    connectCheck();
}
async function getBalance(address) {
    const provider = new ethers.providers.Web3Provider(window.ethereum);
    const balance = await provider.getBalance(address);
    const balanceInEth = ethers.utils.formatEther(balance);
    userEthBalance = balanceInEth;
    console.log('User Eth Balance is ' + balanceInEth);
    return userEthBalance;
}
async function connectAccount() {
    if (typeof window.ethereum !== "undefined") {
        try {
            await connecActWallet();
            await addTokenToWallet();
            userEthBalance = await getBalance(userAddress);
            IsDisabled = false;
            localStorage.setItem("ConnectedAddress", accounts[0]);
            console.log(accounts[0]);
            AccountInfo = accounts[0];
            const contractAddress = buynacContAdd;
            console.log('contractAddress ' + contractAddress);
            const abi = buynacabi;
            const contract = new ethers.Contract(contractAddress, abi, signerPro);
            holdContract = contract;
            localStorage.setItem("holdContract", contract);
            contract.LatestPriceForEthUsdt().then(function (LatestPriceMsgR) {
                console.log(LatestPriceMsgR);
                minEthtoBuy = LatestPriceMsgR[1];
                currentEthPrice = LatestPriceMsgR[3];
                currentTechnoSavvyPrice = LatestPriceMsgR[5];
                currentUSDTPrice = 1;
                var usdtM = LatestPriceMsgR[1] / LatestPriceMsgR[3];
                minUSDTtoBuy = usdtM;
                buyFromEth(userEthBalance);
                $('#receiverTechnoSavvy').val(TechnoSavvyCount);
            }).catch(error => {
                CreateToasterMessage("Error", 'connectAccount ->' + error.message, "error");
                console.log('connectAccount ->' + error.message);
            }
            );
            document.getElementById("receiverCurr").value = userEthBalance;
        } catch (e) {
            CreateToasterMessage("Error", 'connectAccount ->' + e.message, "error");
            console.log('connectAccount ->' + e.message);
            console.log(e);
        }
    } else {
        isConnected(false);
        localStorage.setItem("isConnected", "false");
    }
}
$(document).ready(function () {
    if (typeof window.ethereum !== "undefined") {
        window.ethereum.on('accountsChanged', async function (accounts) {
            console.log(accounts[0]);
            await getAccountDetails(accounts[0]);
            document.getElementById("receiverCurr").value = userEthBalance;
            buyFromEth(userEthBalance);
            $('#receiverTechnoSavvy').val(TechnoSavvyCount);
        });
        hasMetamask = true;
        isDisabled = true;  
        console.log("Injected Web3 Wallet is installed!");
    } else {
        isDisabled = false;
        localStorage.setItem("ConnectedAddress", "N/A");
        localStorage.setItem("isConnected", "false");
        alert("Please install meta mask first");
    }
    if (isConnected) {
        AccountInfo = res;
        IsConnectedMeta = true;
    }
    var selCurr = document.getElementById('selCurr');
    var trustwalletlst = currOptions;
    selCurr.innerText = '';
    for (var i = 0; i < trustwalletlst.length; i++) {
        selCurr.options[selCurr.options.length] = new Option(currOptions[i].label, trustwalletlst[i].value);
    }
    document.getElementById("receiverCurr").value = userEthBalance;
    connectCheck();
    AcAllowance();
    IsLoading();
    CurrencyName = $('#selCurr').val();
    ExecutteStep(0);
});
function AcAllowance() {
    if (isAllowance) {
        $("#divTechnoSavvyReceived").removeClass("d-none");
        $("#divformexchange").addClass("d-none");
    } else {
        $("#divTechnoSavvyReceived").addClass("d-none");
        $("#divformexchange").removeClass("d-none");
    }
}
function connectCheck() {
    if (isConnected) {
        $("#btnBuyNav").removeClass("d-none");
        $("#myDIV").removeClass("div-to-toggle");
        $("#btnConnect").text("Connected");
        $("#btnConnect").removeAttr("data-bs-target");
    } else {
        $("#btnBuyNav").addClass("d-none");
    }
}
function OpenSpinner() {
    $("#divspinner").removeClass("d-none");
    $("#divman").addClass("d-none");
}
function CloseSpinner() {
    $("#divspinner").addClass("d-none");
    $("#divman").removeClass("d-none");
}
function IsLoading() {
    if (isLoading) {
        OpenSpinner();
    } else {
        CloseSpinner();
    }
}
function IsLoadingModel(_istrue) {
    if (_istrue) {
        $("#divspinnerNew").addClass('loading-spinner_2').removeClass('d-none');
    } else {
        $("#divspinnerNew").addClass('d-none');
    }
}
var tokenimgUrl = window.location.href + 'images//TechnoSavvyCoinSvg.svg';
async function addTokenToWallet() {
    ethereum.request({
        method: 'wallet_watchAsset',
        params: {
            type: 'ERC20',
            options: {
                address: TechnoSavvyTokenAdd,
                symbol: 'TechnoSavvy',
                decimals: 18,
                image: tokenimgUrl,
            },
        },
    }).then((success) => {
        if (success) {
            CreateToasterMessage("Token Add", 'addTokenToWallet -> Token Add successfuly', "success");
        } else {
            throw new Error('Something went wrong.')
        }
    }
    ).catch(console.error);
}
function StekTechnoSavvy() {
    if (isConnected) {
        nvcChange("5000");
    } else {
        $("#ConnectModel").modal("show");
    }
}

var hasMetamask = false;
var isDisabled = false;

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
        hasMetamask = false;
        localStorage.setItem("ConnectedAddress", "N/A");
        localStorage.setItem("isConnected", "false");
        alert("Please install meta mask first");
    }
});
async function getAccountDetails(acc) {
    localStorage.setItem("ConnectedAddress", acc);
    userAddress = acc;
    userEthBalance = await getBalance(acc);
    txtVal = userEthBalance;
    ethPrice = userEthBalance;
    document.getElementById("txtAccId").value = acc;
}
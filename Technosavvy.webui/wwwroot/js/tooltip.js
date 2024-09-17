var popoverList = [];
var popoverTriggerList = [];
$(document).ready(function () {
    popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
    popoverList = popoverTriggerList.map(function(popoverTriggerEl)
    {
        return new bootstrap.Popover(popoverTriggerEl);
    });
});
$(function () {
    "use strict";

    $('#cardNumber').payment('formatCardNumber');
    $("#expityMonth").numeric({
        allowMinus: false,
        allowThouSep: false,
        allowDecSep: false,
        maxDigits: 2
    });
    $("#expityYear").numeric({
        allowMinus: false,
        allowThouSep: false,
        allowDecSep: false,
        maxDigits: 4
    });
    $("#cvCode").numeric({
        allowMinus: false,
        allowThouSep: false,
        allowDecSep: false,
        maxDigits: 3
    });
    $("#cardHolder").alphanum({
        allow: ' ',
        allowNumeric: false,
        allowOtherCharSets: false
});

    var $paymentForm = $('#paymentForm');
    var checkout = new cp.Checkout("test_api_00000000000000000000002", $paymentForm[0]);

    $paymentForm.submit(function () {
        $paymentForm.find('input').parent('div').removeClass('has-error');
        var cardHolderName = $('#cardHolder').val();
        var cryptoPacket = checkout.createCryptogramPacket();
        if (cryptoPacket.success) {
            $('#CardHolderName').val(cardHolderName);
            $('#CardCryptogramPacket').val(cryptoPacket.packet);
        } else {
            $.each(cryptoPacket.messages, function(key, value) {
                $paymentForm.find('input[data-cp=' + key + ']').parent('div').addClass('has-error');
            });
            event.preventDefault();
        }
    });

});
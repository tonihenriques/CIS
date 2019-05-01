
jQuery(function ($) {

    //$("input[id*='txtCNPJ']").inputmask({
    //    mask: ['999.999.999-99', '99.999.999/9999-99'],
    //    keepStatic: true
    //});





    //$("#txtCNPJ").keydown(function () {
    //    try {
    //        $("#txtCNPJ").unmask();
    //    } catch (e) { }

    //    $("#txtCNPJ").inputmask({
    //        mask: ['999.999.999-99', '99.999.999/9999-99'],
    //        keepStatic: true
    //    });

    //});



    //var $j = jQuery.noConflict();

    //$j(function () {
    //    $j("#txtCNPJ").mask("99.999.999/9999-99");
    //});


});


function OnSuccessCadastrarFornecedor(data) {
    $('#formCadastroFornecedor').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginCadastrarFornecedor() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formCadastroFornecedor").css({ opacity: "0.5" });
}




//var scripts = ["/Components/jquery.maskedinput/dist/jquery.maskedinput.js", "/Components/bootstrap-datepicker/dist/js/bootstrap-datepicker.js", "/Components/bootstrap-datepicker/dist/locales/bootstrap-datepicker.pt-BR.min.js"]
$('.page-content-area').ace_ajax('loadScripts', scripts, function () {

    jQuery(function ($) {

        $('#txtCNPJ').mask('99.999.999/9999-99');
        $('#Telefone').mask('99 9 9999-9999');

        $('.date-picker').datepicker({
            autoclose: true,
            todayhighlight: true,
            language: "pt-BR"
        })
            .next().on(ace.click_event, function () {
                $(this).prev().focus();
            });

    });

});

function OnBeginAtualizarFornecedor() {
    $('.page-content-area').ace_ajax('startLoading');
}

function OnSuccessAtualizarFornecedor(data) {
    $('.page-content-area').ace_ajax('stopLoading', true);
    TratarResultadoJSON(data.resultado);
}







function OnSuccessAtualizarFornecedor(data) {
    $('#formEdicaoFornecedor').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginAtualizarFornecedor() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formEdicaoFornecedor").css({ opacity: "0.5" });
}
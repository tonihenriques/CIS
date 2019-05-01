AplicaValidacaoCPF();

jQuery(function ($) {

    $("#txtCPF").keydown(function () {
        try {
            $("#txtCPF").unmask();
        } catch (e) { }

        $("#txtCPF").inputmask("999.999.999-99");

    });

function OnSuccessAtualizarEmpregado(data) {
    $('#formEdicaoEmpregado').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginAtualizarEmpregado() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formEdicaoEmpregado").css({ opacity: "0.5" });
}
});
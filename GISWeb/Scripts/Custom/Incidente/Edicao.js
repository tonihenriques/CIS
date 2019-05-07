jQuery(function ($) {

  

});

function OnBeginAtualizarEmpregado() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formEdicaoEmpregado").css({ opacity: "0.5" });
}

function OnSuccessAtualizarEmpregado(data) {
    $('#formEdicaoEmpregado').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

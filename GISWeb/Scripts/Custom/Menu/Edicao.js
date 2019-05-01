
function OnSuccessAtualizarMenu(data) {
    $('#formEdicaoMenu').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginAtualizarMenu() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formEdicaoMenu").css({ opacity: "0.5" });
}
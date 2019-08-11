jQuery(function ($) {

    $.validator.unobtrusive.parse(document);

    DatePTBR();

    var date = new Date();
    date.setDate(date.getDate());

    $('.date-picker').datepicker({
        autoclose: true,
        todayHighlight: true,
        language: 'pt-BR',
        maxDate: date
    }).next().on(ace.click_event, function () {
        $(this).prev().focus();
    });



    $('#HoraIncidente').timepicker({
        minuteStep: 1,
        showSeconds: false,
        showMeridian: false,
        disableFocus: true,
        icons: {
            up: 'fa fa-chevron-up',
            down: 'fa fa-chevron-down'
        }
    }).on('focus', function () {
        $('#HoraIncidente').timepicker('showWidget');
    }).next().on(ace.click_event, function () {
        $(this).prev().focus();
    });

    Chosen();


    $("#UKOrgao").change(function () {

        if ($(this).val() == "") {
            $("#UKDiretoria").val("");
        }
        else {

            $(".LoadingLayout").show();
            $('.page-content-area').ace_ajax('startLoading');

            $.ajax({
                method: "POST",
                url: "/Departamento/BuscarDiretoriaPorOrgao",
                data: { ukDepartamento: $(this).val() },
                error: function (erro) {
                    $(".LoadingLayout").hide();
                    $('.page-content-area').ace_ajax('stopLoading', true);
                    ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
                },
                success: function (content) {
                    $('.page-content-area').ace_ajax('stopLoading', true);
                    $(".LoadingLayout").hide();

                    var resultado = content.resultado;

                    if (resultado.Erro != null && resultado.Erro != undefined && resultado.Erro != "") {
                        ExibirMensagemDeErro(resultado.Erro);
                    }
                    else if (resultado.Alerta != null && resultado.Alerta != undefined && resultado.Alerta != "") {
                        ExibirMensagemDeAlerta(resultado.Alerta);
                    }
                    else {
                        if (resultado.Conteudo.indexOf("$") != -1) {
                            $("#UKDiretoria").val(resultado.Conteudo.substring(resultado.Conteudo.indexOf("$") + 1));
                        }
                        else {
                            $("#UKDiretoria").val(resultado.Conteudo);
                        }
                    }

                }
            });

        }

    });



});

function OnBeginCadastrarAcidente(jqXHR, settings) {

    if ($("#Acidente").prop("checked") == true) {
        $("#Acidente").val(true);
    }
    else {
        $("#Acidente").val(false);
    }

    if ($("#AcidenteFatal").prop("checked") == true) {
        $("#AcidenteFatal").val(true);
    }
    else {
        $("#AcidenteFatal").val(false);
    }

    if ($("#AcidentePotencial").prop("checked") == true) {
        $("#AcidentePotencial").val(true);
    }
    else {
        $("#AcidentePotencial").val(false);
    }

    if ($("#AcidenteGraveIP102").prop("checked") == true) {
        $("#AcidenteGraveIP102").val(true);
    }
    else {
        $("#AcidenteGraveIP102").val(false);
    }


    var form = $("#formCadastroAcidente");
    settings.data = form.serialize();

    $(".LoadingLayout").show();
    $('.page-content-area').ace_ajax('startLoading');

}

function OnSuccessCadastrarAcidente(data) {
    $(".LoadingLayout").hide();
    $('.page-content-area').ace_ajax('stopLoading', true);

    TratarResultadoJSON(data.resultado);
}

function OnFailureCadastrarAcidente() {
    $('.page-content-area').ace_ajax('stopLoading', true);
}
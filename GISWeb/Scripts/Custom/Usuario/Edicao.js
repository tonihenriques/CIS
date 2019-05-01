
jQuery(function ($) {

    $("#txtCPF").keydown(function () {
        try {
            $("#txtCPF").unmask();
        } catch (e) { }

        $("#txtCPF").inputmask("999.999.999-99");

    });

    $("#ddlEmpresa").change(function () {

        if ($(this).val() != "") {

            $('#UKDepartamento').empty();
            $('#UKDepartamento').append($('<option></option>').val("").html("Aguarde ..."));
            $("#UKDepartamento").attr("disabled", true);

            $.ajax({
                method: "POST",
                url: "/Departamento/ListarDepartamentosPorEmpresa",
                data: { idEmpresa: $(this).val() },
                error: function (erro) {
                    ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
                },
                success: function (content) {
                    if (content.resultado.length > 0) {
                        $("#UKDepartamento").attr("disabled", false);
                        $('#UKDepartamento').empty();
                        $('#UKDepartamento').append($('<option></option>').val("").html("Selecione um departamento"));
                        for (var i = 0; i < content.resultado.length; i++) {
                            $('#UKDepartamento').append(
                                $('<option></option>').val(content.resultado[i].UniqueKey).html(content.resultado[i].Sigla)
                            );
                        }
                    }
                    else {
                        $('#UKDepartamento').empty();
                        $('#UKDepartamento').append($('<option></option>').val("").html("Nenhum departamento encontrado para esta empresa"));
                    }
                }
            });
        }
        else {
            $('#UKDepartamento').empty();
            $('#UKDepartamento').append($('<option></option>').val("").html("Selecione antes uma Empresa..."));
            $("#UKDepartamento").attr("disabled", true);
        }
    });

});

function OnSuccessAtualizarUsuario(data) {
    $('#formEdicaoUsuario').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginAtualizarUsuario() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formEdicaoUsuario").css({ opacity: "0.5" });
}
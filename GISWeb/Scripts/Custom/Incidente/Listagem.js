jQuery(function ($) {

    AplicajQdataTable("dynamic-table", [{ "bSortable": false }, null,null,null,null, { "bSortable": false }], false, 20);

});

function BuscarRegistroPorID(RegistroID) {

    $(".LoadingLayout").show();

    $.ajax({
        method: "POST",
        url: "/Registro/BuscarEmpregadoPorID",
        data: { idRegistro: RegistroID },
        error: function (erro) {
            $(".LoadingLayout").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $(".LoadingLayout").hide();

            if (content.data != null) {
                bootbox.dialog({
                    message: content.data,
                    title: "<span class='bigger-110'>Detalhes do Registro</span>",
                    backdrop: true,
                    locale: "br",
                    buttons: {},
                    onEscape: true
                });
            }
            else {
                TratarResultadoJSON(content.resultado);
            }

        }
    });

}

function DeletarEmpregado(IDEmpregado, Nome) {

    var callback = function () {
        $('.LoadingLayout').show();
        $('#dynamic-table').css({ opacity: "0.5" });

        $.ajax({
            method: "POST",
            url: "/Empresa/Terminar",
            data: { IDEmpregado: IDEmpregado },
            error: function (erro) {
                $(".LoadingLayout").hide();
                $("#dynamic-table").css({ opacity: '' });
                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
            },
            success: function (content) {
                $('.LoadingLayout').hide();
                $("#dynamic-table").css({ opacity: '' });

                TratarResultadoJSON(content.resultado);

                if (content.resultado.Sucesso != null && content.resultado.Sucesso != "") {
                    $("#linha-" + IDEmpregado).remove();
                }
            }
        });
    };

    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir o empregado '" + Nome + "'?", "Exclusão de Empregado", callback, "btn-danger");

}
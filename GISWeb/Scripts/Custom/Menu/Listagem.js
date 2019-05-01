jQuery(function ($) {

    AplicajQdataTable("dynamic-table", [null, null, null, null, null, { "bSortable": false }], false, 20);

});

function DeletarMenu(IDMenu, Nome) {

    var callback = function () {
        $('.LoadingLayout').show();
        $('#dynamic-table').css({ opacity: "0.5" });

        $.ajax({
            method: "POST",
            url: "/Menu/Terminar",
            data: { IDMenu: IDMenu },
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
                    $("#linha-" + IDMenu).remove();
                }
            }
        });
    };

    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir o menu '" + Nome + "'?", "Exclusão de Menu", callback, "btn-danger");

}

function BuscarPerfis(IDMenu, Nome) {
    
    $(".LoadingLayout").show();
    $.post('/PerfilMenu/BuscarPerfisPorMenu', { IDMenu: IDMenu }, function (partial) {
        $(".LoadingLayout").hide();
        
        if (partial.resultado != undefined) {
            TratarResultadoJSON(partial.resultado);
        }
        else {
            bootbox.dialog({
                message: partial.data,
                title: "<span class='bigger-100'>Quais perfis terão acesso ao menu '" + Nome + "'?</span>",
                backdrop: true,
                locale: "br",
                buttons: {},
                onEscape: true
            });

            if (parseInt(partial.perfis) > 0) {

                var oTable1 = AplicajQdataTable("tablePerfisPorMenu", [{ "bSortable": false }, null], false, 25);

                TableTools.classes.container = "btn-group btn-overlap";

                //initiate TableTools extension
                var tableTools_obj = new $.fn.dataTable.TableTools(oTable1, {
                    "sRowSelector": "td:not(:last-child)",
                    "sRowSelect": "multi",
                    "fnRowSelected": function (row) {
                        //check checkbox when row is selected
                        try { $(row).find('input[type=checkbox]').get(idxCol).checked = true }
                        catch (e) { }
                    },
                    "fnRowDeselected": function (row) {
                        //uncheck checkbox
                        try { $(row).find('input[type=checkbox]').get(idxCol).checked = false }
                        catch (e) { }
                    },

                    "sSelectedClass": "success"
                });

                //$('#dynamic-table1 > thead > tr > th input[type=checkbox]').on('click', function () {

                //    idxCol = $(this).attr("rel");

                //    var th_checked = this.checked;//checkbox inside "TH" table header
                //    $(this).closest('table').find('tbody > tr').each(function () {
                //        var row = this;
                //        if (th_checked) tableTools_obj.fnSelect(row);
                //        else tableTools_obj.fnDeselect(row);
                //    });

                //    SalvarPermissoes(th_checked, $(this).attr("id"), "");
                //});

                //$('#dynamic-table1').on('click', 'td input[type=checkbox]', function () {
                //var row = $(this).closest('tr').get(0);
                //if (!this.checked) tableTools_obj.fnSelect(row);
                //else tableTools_obj.fnDeselect($(this).closest('tr').get(0));
                //});

            }

        }

    });

}

function SalvarPerfilParaOMenu(Acao, Perfil, Menu) {

    $(".LoadingLayout").show();
    $.post('/PerfilMenu/SalvarEdicaoVinculo', { Acao: Acao, Perfil: Perfil, Menu: Menu }, function (partial) {
        $(".LoadingLayout").hide();

        

    });


}
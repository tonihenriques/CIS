jQuery(function ($) {
    AplicajQdataTable("tblSheets", [null, null, null, null, { "bSortable": false }], false, 20);
});

function ProcessarTodasAsSheets() {

    $("#ContentAjax").html("");
    $(".LoadingLayout").show();
    
    $.ajax({
        method: "POST",
        url: "/Ferramentas/LoaderAllSheetsFromTabelasAuxiliares",
        data: {  },
        error: function (erro) {
            $(".LoadingLayout").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $(".LoadingLayout").hide();
            
            $("#ContentAjax").show();
            $("#ContentAjax").html(content);
        }
    });

}
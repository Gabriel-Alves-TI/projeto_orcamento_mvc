$(document).ready(function() {
    $('#Orcamentos').DataTable({
        language: {
            "decimal":        "",
            "emptyTable":     "No data available in table",
            "info":           "Mostrando _START_ de _END_ de _TOTAL_ dados",
            "infoEmpty":      "Mostrando 0 de 0 de 0 dados",
            "infoFiltered":   "(filtered from _MAX_ total entries)",
            "infoPostFix":    "",
            "thousands":      ",",
            "lengthMenu":     "Mostrar _MENU_ linhas",
            "loadingRecords": "Carregando...",
            "processing":     "",
            "search":         "Buscar:",
            "zeroRecords":    "Nenhum resultado encontrado!",
            "paginate": {
                "first":      "Primeira",
                "last":       "Última",
                "next":       "Próxima",
                "previous":   "Anterior"
            },
            "aria": {
                "orderable":  "Order by this column",
                "orderableReverse": "Reverse order this column"
            }
        }
    });
})

$(document).ready(function() {
    $('#Recibos').DataTable({
        language: {
            "decimal":        "",
            "emptyTable":     "No data available in table",
            "info":           "Mostrando _START_ de _END_ de _TOTAL_ dados",
            "infoEmpty":      "Mostrando 0 de 0 de 0 dados",
            "infoFiltered":   "(filtered from _MAX_ total entries)",
            "infoPostFix":    "",
            "thousands":      ",",
            "lengthMenu":     "Mostrar _MENU_ linhas",
            "loadingRecords": "Carregando...",
            "processing":     "",
            "search":         "Buscar:",
            "zeroRecords":    "Nenhum resultado encontrado!",
            "paginate": {
                "first":      "Primeira",
                "last":       "Última",
                "next":       "Próxima",
                "previous":   "Anterior"
            },
            "aria": {
                "orderable":  "Order by this column",
                "orderableReverse": "Reverse order this column"
            }
        }
    });
})

$(document).ready(function() {
    $('#Clientes').DataTable({
        language: {
            "decimal":        "",
            "emptyTable":     "No data available in table",
            "info":           "Mostrando _START_ de _END_ de _TOTAL_ dados",
            "infoEmpty":      "Mostrando 0 de 0 de 0 dados",
            "infoFiltered":   "(filtered from _MAX_ total entries)",
            "infoPostFix":    "",
            "thousands":      ",",
            "lengthMenu":     "Mostrar _MENU_ linhas",
            "loadingRecords": "Carregando...",
            "processing":     "",
            "search":         "Buscar:",
            "zeroRecords":    "Nenhum resultado encontrado!",
            "paginate": {
                "first":      "Primeira",
                "last":       "Última",
                "next":       "Próxima",
                "previous":   "Anterior"
            },
            "aria": {
                "orderable":  "Order by this column",
                "orderableReverse": "Reverse order this column"
            }
        }
    });
})

$(document).ready(function() {
    $('#ListaItens').DataTable({
        language: {
            "decimal":        "",
            "emptyTable":     "Nenhum item encontrado!",
            "info":           "Mostrando _START_ de _END_ de _TOTAL_ dados",
            "infoEmpty":      "Mostrando 0 de 0 de 0 dados",
            "infoFiltered":   "(filtered from _MAX_ total entries)",
            "infoPostFix":    "",
            "thousands":      ",",
            "lengthMenu":     "Mostrar _MENU_ linhas",
            "loadingRecords": "Carregando...",
            "processing":     "",
            "search":         "Buscar:",
            "zeroRecords":    "Nenhum resultado encontrado!",
            "paginate": {
                "first":      "Primeira",
                "last":       "Última",
                "next":       "Próxima",
                "previous":   "Anterior"
            },
            "aria": {
                "orderable":  "Order by this column",
                "orderableReverse": "Reverse order this column"
            }
        },
        searching: false,
        paging: false,
        info: false,
        aria: false,
        ordering: false,
    });
})

function inicializarTabelaClientes() {

    if ($.fn.DataTable.isDataTable('#listaClientes')) {
        $('#listaClientes').DataTable().clear().destroy();
    }

    $('#listaClientes').DataTable({
        language: {
            "decimal":        "",
            "emptyTable":     "Nenhum item encontrado!",
            "info":           "Mostrando _START_ de _END_ de _TOTAL_ dados",
            "infoEmpty":      "Mostrando 0 de 0 de 0 dados",
            "infoFiltered":   "(filtered from _MAX_ total entries)",
            "infoPostFix":    "",
            "thousands":      ",",
            "lengthMenu":     "Mostrar _MENU_ linhas",
            "loadingRecords": "Carregando...",
            "processing":     "",
            "search":         "Buscar:",
            "zeroRecords":    "Nenhum resultado encontrado!",
            "paginate": {
                "first":      "Primeira",
                "last":       "Última",
                "next":       "Próxima",
                "previous":   "Anterior"
            },
            "aria": {
                "orderable":  "Order by this column",
                "orderableReverse": "Reverse order this column"
            }
        },
        ajax: {
            url: '/Cliente/ListarClientes',
            dataSrc: ''
        },
        columns: [
            { data: 'nome' },
            {
                data: null,
                orderable: false,
                searchable: false,
                render: function (data, type, row) {
                    return `
                        <button class="btn btn-sm btn-primary btnSelecionar"
                                onclick="selecionarCliente(${row.id}, '${row.nome}')">
                            Selecionar
                        </button>`;
                }
            }
        ],
        ordering: false
    });
}

$('#modalCliente').on('shown.bs.modal', function () {
    inicializarTabelaClientes();
});
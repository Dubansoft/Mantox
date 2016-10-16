jQuery(function ($) {
            var grid_selector = "#grid-table";
            var pager_selector = "#grid-pager";
            var modal_iframe = $("#modalPrincipal");
            var modal_confirmacion = $("#modalConfirmacion");
            var parent_column = $(grid_selector).closest('[class*="box-body"]');

            //resize to fit page size
            $(window).on('resize.jqGrid', function () {
                setTimeout(function () {
                    $(grid_selector).jqGrid('setGridWidth', parent_column.width());
                }, 1000);
            });
            //resize on sidebar
            $("#toggleSidebar").click(function() {
                setTimeout(function () {
                    $(grid_selector).jqGrid('setGridWidth', parent_column.width());
                }, 500);
                console.log("test");
            });
            //resize on sidebar collapse/expand
            $(document).on('settings.ace.jqGrid', function (ev, event_name, collapsed) {
                if (event_name === 'sidebar_collapsed' || event_name === 'main_container_fixed') {
                    //setTimeout is for webkit only to give time for DOM changes and then redraw!!!
                    setTimeout(function () {
                        $(grid_selector).jqGrid('setGridWidth', parent_column.width());
                    }, 500);
                }
            })

            //if your grid is inside another element, for example a tab pane, you should use its parent's width:
            $(window).on('resize.jqGrid', function () {
                var parent_width = $(grid_selector).closest('.box-body').width();
                setTimeout(function () {
                    $(grid_selector).jqGrid('setGridWidth', parent_column.width());
                }, 500);
            })
            //and also set width when tab pane becomes visible
            $('#myTab a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
              if($(e.target).attr('href') == '#mygrid') {
                  var parent_width = $(grid_selector).closest('.box-body').width();
                  setTimeout(function () {
                      $(grid_selector).jqGrid('setGridWidth', parent_column.width());
                  }, 500);
              }
            })

            //Configuración de la tabla
            jQuery(grid_selector).jqGrid({
                url: grid_data_url,
                mtype: method_type,
    			datatype: data_type,

    			subGrid: show_subgrid,
                loadonce: load_once,
                subGridModel: subgrid_model,

                subGridOptions: {
                    plusicon: "ace-icon fa fa-plus center bigger-110 blue",
                    minusicon: "ace-icon fa fa-minus center bigger-110 blue",
                    openicon: "ace-icon fa fa-chevron-right center orange"
                },

                ////for this example we are using local data
                subGridRowExpanded: subGridRow_Expanded,

                height: grid_height,
                colNames: column_names,
                colModel: column_model ,

                viewrecords: true,

                rowNum: row_number_width,
                rowList: [2, 3, 5, 10, 20, 30, 50, 100, 200, 300, 500, 1000],
                pager: pager_selector,
                rownumbers: true,
                rownumWidth: 25,
                autowidth: true,
                toppager: top_pager,
                shrinkToFit: true,
                multiselect: multi_select,

                loadComplete: function () {
                    var table = this;
                    setTimeout(function () {
                        styleCheckbox(table);
                        updateActionIcons(table);
                        updatePagerIcons(table);
                        enableTooltips(table);
                    }, 0);
                },

                caption: grid_caption,
                grouping: grid_grouping,
                groupingView : {
                    groupField: grid_group_fields,
                    groupDataSorted : true,
                    plusicon : 'fa fa-chevron-down bigger-110',
                    minusicon : 'fa fa-chevron-up bigger-110'
                },
                jsonReader: {
                    root: "rows",
                    page: "page",
                    total: "total",
                    records: "records"
                }

            });

            var timer;
            //Esta función recarga la tabla con los resultados iniciales cuando se limpia el formulario de búsqueda
            $('#search_cells_form').on('reset', function (e) {
                if (timer) { clearTimeout(timer); }
                timer = setTimeout(function(){
                    timer = null;
                    $(grid_selector).jqGrid('setGridParam', { search: false, postData: { "filters": "" } }).trigger("reloadGrid");
                },400);
            });

            //Esta función recarga la tabla con los resultados de búsqueda ingresados en la casilla de búsqueda, si no hay texto en la casilla, recarga la tabla con los resultados iniciales.
            $("#search_cells").on("keyup", function () {
                var searchTerm = $("#search_cells").val().trim();
                if (searchTerm.length == 0) {

                    if (timer) { clearTimeout(timer); }
                    timer = setTimeout(function () {
                        $(grid_selector).jqGrid('setGridParam', { search: false, postData: { "filters": "" } }).trigger("reloadGrid");
                    }, 400);

                    return;
                }
                var self = this;
                if(timer) { clearTimeout(timer); }
                timer = setTimeout(function(){
                    $("#grid-table").jqGrid('filterInput', searchTerm); //self.value);
                },400);
            });

            //Esta función abre una ventana modal para editar
            $('#search_cells_form').on('submit', function (e) {
                if (timer) { clearTimeout(timer); }
                timer = setTimeout(function () {
                    timer = null;
                }, 0);
            });


            $(window).triggerHandler('resize.jqGrid');//trigger window resize to make the grid get the correct size

            //navButtons
            jQuery(grid_selector).jqGrid('navGrid', pager_selector,

                {   //navbar options
                    edit: false,
                    editicon: 'ace-icon fa fa-pencil blue',
                    add: false,
                    addicon: 'ace-icon fa fa-plus-circle purple',
                    del: false,
                    delicon: 'ace-icon fa fa-trash-o red',
                    search: false,
                    searchicon: 'ace-icon fa fa-search orange',
                    refresh: true,
                    refreshicon: 'ace-icon fa fa-refresh green',
                    view: true,
                    viewicon: 'ace-icon fa fa-search-plus grey',
                },
                {
                    //edit record form
                    //closeAfterEdit: true,
                    //width: 700,
                    recreateForm: true,
                    beforeShowForm: function (e) {
                        var form = $(e[0]);
                        form.closest('.ui-jqdialog').find('.ui-jqdialog-titlebar').wrapInner('<div class="widget-header" />')

                        style_edit_form(form);
                    }
                },
                {
                    //new record form
                    //width: 700,
                    closeAfterAdd: true,
                    recreateForm: true,
                    viewPagerButtons: false,
                    beforeShowForm: function (e) {
                        var form = $(e[0]);
                        form.closest('.ui-jqdialog').find('.ui-jqdialog-titlebar')
                        .wrapInner('<div class="widget-header" />')

                        style_edit_form(form);
                    }
                },
                {
                    //delete record form
                    recreateForm: true,
                    beforeShowForm: function (e) {
                        var form = $(e[0]);
                        if (form.data('styled')) return false;

                        form.closest('.ui-jqdialog').find('.ui-jqdialog-titlebar').wrapInner('<div class="widget-header" />')

                        style_delete_form(form);

                        form.data('styled', true);
                    },
                    onClick: function (e) {
                        //alert(1);
                    }
                },
                {
                    //search form
                    recreateForm: true,
                    afterShowSearch: function (e) {
                        var form = $(e[0]);
                        form.closest('.ui-jqdialog').find('.ui-jqdialog-title').wrap('<div class="widget-header" />')
                        style_search_form(form);
                    },
                    afterRedraw: function () {
                        style_search_filters($(this));
                    }
                    //multipleSearch: true
                    //multipleGroup:true,
                    //showQuery: true

                },
                {
                    //view record form
                    recreateForm: true,
                    beforeShowForm: function (e) {
                        var form = $(e[0]);
                        form.closest('.ui-jqdialog').find('.ui-jqdialog-title').wrap('<div class="widget-header" />')

                    }
                }
            )



            function style_edit_form(form) {
                //enable datepicker on "sdate" field and switches for "stock" field
                //form.find('input[name=sdate]').datepicker({ format: 'yyyy-mm-dd', autoclose: true })

                form.find('input[name=stock]').addClass('ace ace-switch ace-switch-5').after('<span class="lbl"></span>');
                //don't wrap inside a label element, the checkbox value won't be submitted (POST'ed)
                //.addClass('ace ace-switch ace-switch-5').wrap('<label class="inline" />').after('<span class="lbl"></span>');


                //update buttons classes
                var buttons = form.next().find('.EditButton .fm-button');
                buttons.addClass('btn btn-sm').find('[class*="-icon"]').hide();//ui-icon, s-icon
                buttons.eq(0).addClass('btn-primary').prepend('<i class="ace-icon fa fa-check"></i>');
                buttons.eq(1).prepend('<i class="ace-icon fa fa-times"></i>')


                buttons = form.next().find('.navButton a');
                buttons.find('.ui-icon').hide();
                buttons.eq(0).append('<i class="ace-icon fa fa-chevron-left"></i>');
                buttons.eq(1).append('<i class="ace-icon fa fa-chevron-right"></i>');
            }

            function style_delete_form(form) {
                var buttons = form.next().find('.EditButton .fm-button');
                buttons.addClass('btn btn-sm btn-white btn-round').find('[class*="-icon"]').hide();//ui-icon, s-icon
                buttons.eq(0).addClass('btn-danger').prepend('<i class="ace-icon fa fa-trash-o"></i>');
                buttons.eq(1).addClass('btn-default').prepend('<i class="ace-icon fa fa-times"></i>')

            }

            function style_search_filters(form) {
                form.find('.delete-rule').val('X');
                form.find('.add-rule').addClass('btn btn-xs btn-primary');
                form.find('.add-group').addClass('btn btn-xs btn-success');
                form.find('.delete-group').addClass('btn btn-xs btn-danger');
            }
            function style_search_form(form) {
                var dialog = form.closest('.ui-jqdialog');
                var buttons = dialog.find('.EditTable')

                buttons.find('.EditButton a[id*="_reset"]').addClass('btn btn-sm btn-info').find('.ui-icon').attr('class', 'ace-icon fa fa-retweet');
                buttons.find('.EditButton a[id*="_query"]').addClass('btn btn-sm btn-inverse').find('.ui-icon').attr('class', 'ace-icon fa fa-comment-o');
                buttons.find('.EditButton a[id*="_search"]').addClass('btn btn-sm btn-purple').find('.ui-icon').attr('class', 'ace-icon fa fa-search');
            }

            function beforeDeleteCallback(e) {
                var form = $(e[0]);
                if (form.data('styled')) return false;

                form.closest('.ui-jqdialog').find('.ui-jqdialog-titlebar').wrapInner('<div class="widget-header" />')

                style_delete_form(form);

                form.data('styled', true);
            }

            function beforeEditCallback(e) {
                var form = $(e[0]);
                form.closest('.ui-jqdialog').find('.ui-jqdialog-titlebar').wrapInner('<div class="widget-header" />')

                style_edit_form(form);
            }



            //it causes some flicker when reloading or navigating grid
            //it may be possible to have some custom formatter to do this as the grid is being created to prevent this
            //or go back to default browser checkbox styles for the grid
            function styleCheckbox(table) {
                /**
                    $(table).find('input:checkbox').addClass('ace')
                    .wrap('<label />')
                    .after('<span class="lbl align-top" />')


                    $('.ui-jqgrid-labels th[id*="_cb"]:first-child')
                    .find('input.cbox[type=checkbox]').addClass('ace')
                    .wrap('<label />').after('<span class="lbl align-top" />');
                */
            }


            //unlike navButtons icons, action icons in rows seem to be hard-coded
            //you can change them like this in here if you want
            function updateActionIcons(table) {
                /**
                var replacement =
                {
                    'ui-ace-icon fa fa-pencil' : 'ace-icon fa fa-pencil blue',
                    'ui-ace-icon fa fa-trash-o' : 'ace-icon fa fa-trash-o red',
                    'ui-icon-disk' : 'ace-icon fa fa-check green',
                    'ui-icon-cancel' : 'ace-icon fa fa-times red'
                };
                $(table).find('.ui-pg-div span.ui-icon').each(function(){
                    var icon = $(this);
                    var $class = $.trim(icon.attr('class').replace('ui-icon', ''));
                    if($class in replacement) icon.attr('class', 'ui-icon '+replacement[$class]);
                })
                */
            }

            //replace icons with FontAwesome icons like above
            function updatePagerIcons(table) {
                var replacement =
                {
                    'ui-icon-seek-first': 'ace-icon fa fa-angle-double-left bigger-140',
                    'ui-icon-seek-prev': 'ace-icon fa fa-angle-left bigger-140',
                    'ui-icon-seek-next': 'ace-icon fa fa-angle-right bigger-140',
                    'ui-icon-seek-end': 'ace-icon fa fa-angle-double-right bigger-140'

                };
                $('.ui-pg-table:not(.navtable) > tbody > tr > .ui-pg-button > .ui-icon').each(function () {
                    var icon = $(this);
                    var $class = $.trim(icon.attr('class').replace('ui-icon', ''));

                    if ($class in replacement) icon.attr('class', 'ui-icon ' + replacement[$class]);
                })
            }

            function enableTooltips(table) {
                $('.navtable .ui-pg-button').tooltip({ container: 'body' });
                $(table).find('.ui-pg-div').tooltip({ container: 'body' });
            }

            //var selr = jQuery(grid_selector).jqGrid('getGridParam','selrow');

            $(document).one('ajaxloadstart.page', function (e) {
                $(grid_selector).jqGrid('GridUnload');
                $('.ui-jqdialog').remove();
            });

            //Función para obtener el id seleccionado de la tabla
            var idSeleccionado = function obtenerIdSeleccionado() {
                var grid = $(grid_selector);
                var rowKey = grid.jqGrid('getGridParam', "selrow");

                if (rowKey) {
                    return rowKey;
                }
                else {
                    crearConfirmacionModal("Error", "Debe seleccionar un elemento antes de continuar", "modalOk");
                    return false;
                }
            }

            var dpe = "c4ca4238a0b923820dcc509a6f75849b";
            var ape = "c81e728d9d4c2f636f067f89cc14862c";
            var dpei = 1;
            var apei = 2;


    //Validamos los permisos de usuario
            if ((pex == dpe && pexi == dpei) || (pex == ape && pexi == apei)) {

                //Custom icon for * Delete
                $(grid_selector).navButtonAdd(pager_selector, {
                    caption: '',
                    title: "Eliminar",
                    buttonicon: "ace-icon fa fa-trash-o red",
                    onClickButton: function () {
                        if (idSeleccionado() == false) { return; }

                        $("#modalResponseSi").bind("click", function () {

                            var newUrl = base_url + grid_controller + '/Eliminar/' + idSeleccionado();

                            $.ajax({
                                url: newUrl,
                                fail: function (result) {
                                    setTimeout(function () {
                                        crearConfirmacionModal("Fall&oacute; la eliminaci&oacute;n", "No se ha eliminado el elemento", "modalOk");
                                    }, 500);
                                },
                                statusCode: {
                                    404: function () {
                                        setTimeout(function () {
                                            crearConfirmacionModal("Error 404", "No se ha encontrado la p&aacute;gina", "modalOk");
                                        }, 500);
                                    },
                                    200: function () {
                                        $(grid_selector).trigger('reloadGrid');
                                    }
                                }
                            });

                            $("#modalResponseSi").unbind("click");

                        });

                        crearConfirmacionModal("Eliminar " + grid_controller, "&iquest;Confirma que desea eliminar este/a " + grid_controller + "?", "modalSiNo");
                    },
                    position: "first"
                });



                //Custom icon for Edit Action
                $(grid_selector).navButtonAdd(pager_selector, {
                    caption: '',
                    title: "Editar",
                    buttonicon: "ui-icon ace-icon fa fa-pencil blue",
                    onClickButton: function () {
                        if (idSeleccionado() == false) { return; }
                        var newUrl = base_url + '/' + grid_controller + '/Editar/' + idSeleccionado();
                        crearIframeModal(newUrl);
                    },
                    position: "first"
                });
                //Custom icon for Add Action
                $(grid_selector).navButtonAdd(pager_selector, {
                    caption: '',
                    title: "Nuevo",
                    buttonicon: "ui-icon ace-icon fa fa-plus-circle purple",
                    onClickButton: function () {
                        var newUrl = base_url + '/' + grid_controller + '/Crear';
                        crearIframeModal(newUrl);
                    },
                    position: "first"
                });
            }


            function crearIframeModal(urlString) {
                var modalHeight = $(modal_iframe).height();
                var iframeHtml = '<iframe src="' + urlString + '" style="border:0; margin:0 auto;position:relative;top:0px;left:0px;right:0px;bottom:0px; width: 100%;height:' + modalHeight * 0.84 + 'px;"></iframe>';

                modal_iframe.find('#modal-body-content').html(iframeHtml);
                $(modal_iframe).modal();
            }

            function crearConfirmacionModal(titulo, pregunta, tipo) {

                $("#modalSiNo").hide();
                $("#modalAceptarCancelar").hide();
                $("#modalOk").hide();

                var modalHeight = $(modal_confirmacion).height();

                modal_confirmacion.find('#tituloPregunta').html(titulo);
                modal_confirmacion.find('#cuerpoPregunta').html(pregunta);

                $("#"+tipo).show();


                $(modal_confirmacion).modal();
            }

            $(modal_iframe).on('hidden.bs.modal', function (e) {
                $(grid_selector).trigger('reloadGrid');
            })


});


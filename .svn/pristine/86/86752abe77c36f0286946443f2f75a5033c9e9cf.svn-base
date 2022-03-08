var global = {};
var gallery;

global.setup = {
    ajax: function () {
        $.ajaxSetup({
            // Remove due to not use anti foreign token
            //headers: { 'X-XSRF-TOKEN': $('[name=ape]').val() },
            type: "POST",
            cache: false,
            error: function (xhr, textStatus, errorThrown) {
                //if (window.Setting.isDebug !== true) {
                //    return;
                //}
                try {
                    // Check request already abort => return
                    if (xhr.status === 0) {
                        return;
                    }

                    console.log("[Request Error]", xhr, textStatus, errorThrown);

                    var data = JSON.parse(xhr.responseText);

                    if (data.code) {
                        global.util.notify("Error", data.message, "error");
                    }
                    else {
                        global.util.notify("Error", "System error, please try again or contact administrator!", "error");
                    }
                } catch (e) {
                    global.util.notify("Error", "System error, please try again or contact administrator!", "error");
                }
            }
        });
    },

    autoLoaderEle: function () {
        var $elements = $('[data-plugins*="loader"]');
        $.each($elements,
            function (i, ele) {
                var $ele = $(ele);

                var loaderClass = "m-loader";
                var loaderAt = $ele.data("loader-at") || "right";
                var loaderAtClass = "m-loader--" + loaderAt;
                var loaderSize = $ele.data("loader-size") || "lg";
                var loaderSizeClass = "m-loader--" + loaderSize;
                var loaderSecond = $ele.data("loader-second") || 5; // <= 0 to load forever, not set is 5

                $ele.on({
                    click: function () {
                        $ele.addClass(loaderClass);
                        $ele.addClass(loaderAtClass);
                        $ele.addClass(loaderSizeClass);
                        $ele.addClass("cursor-not-allowed");

                        if (loaderSecond > 0) {
                            setTimeout(function () {
                                $ele.removeClass(loaderClass);
                                $ele.removeClass(loaderAtClass);
                                $ele.removeClass(loaderSizeClass);
                                $ele.removeClass("cursor-not-allowed");
                            }, parseInt(loaderSecond) * 1000)
                        }
                    }
                });
            }
        );
    },

    initSummerNote: function () {
        var $elements = $('[data-plugins*="SummerNote"]');
        $.each($elements,
            function (i, ele) {

                $(ele).summernote({
                    tabsize: 2,
                    height: 400,
                    callbacks: {
                        onImageUpload: function (files, editor, welEditable) {
                            console.log("upload image");
                            $(files).each(function(idx, file) {
                                global.setup.sendFile(file, editor, welEditable, ele);
                            });
                            
                        }
                    }
                });
            }
        );
    },

    sendFile: function (file, editor, welEditable, sender) {
        console.log("upload image to server");
        var data = new FormData();
        data.append("file", file);
        $.ajax({
            data: data,
            type: "POST",
            url: global.endpoint.api.image.uploadMultiPart,
            cache: false,
            contentType: false,
            processData: false,
            success: function (url) {
                console.log(url);
                //editor.insertImage(welEditable, url.url);
                $(sender).summernote('editor.insertImage', url.url);
            }
        });
    },
    confirmPopup: function () {
        var $elements = $('[data-plugins*="confirm"]');

        $.each($elements,
            function (i, ele) {
                var $ele = $(ele);

                //$ele.on({
                //    click: function () {

                //        Swal.fire({
                //            title: $ele.data("confirm-title") || "Are you sure to delete the Item?",
                //            showDenyButton: true,
                //            showCancelButton: false,
                //            confirmButtonText: `Yes`,
                //            denyButtonText: `No`
                //        }).then((result) => {
                //            /* Read more about isConfirmed, isDenied below */
                //            if (result.isConfirmed) {
                //                var action = $ele.data("confirm-yes-callback");
                //                eval(action);
                //                Swal.fire('Deleted!', '', 'success');
                //            } else if (result.isDenied) {
                //                Swal.fire('Delete action was canceled', '', 'info');
                //            }
                //        });

                        
                //    }
                //});


                $ele.on({
                    click: function () {
                        swal({
                            title: $ele.data("confirm-title") || "Delete Confirm",
                            text: $ele.data("confirm-message") || "Are you sure to delete the Item?",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonClass: "btn btn-danger m-btn m-btn--pill m-btn--air m-btn--icon",
                            confirmButtonText: "<span><i class='la la-thumbs-up'></i><span>Yes</span></span>",
                            cancelButtonText: "<span><i class='la la-thumbs-down'></i><span>No</span></span>",
                            cancelButtonClass: "btn btn-secondary m-btn m-btn--pill m-btn--icon",
                            //closeOnConfirm: false,
                            //closeOnCancel: false
                        }).then(function (e) {
                            if (e.value === true) {
                                var action = $ele.data("confirm-yes-callback");
                                eval(action);
                                swal({
                                    title: $ele.data("confirm-yes-title") || "Deleted!",
                                    text: $ele.data("confirm-yes-message") || "Your Item has been deleted!",
                                    type: "success",
                                    timer: 2000
                                });
                            } else {
                                swal({
                                    title: $ele.data("confirm-no-title") || "Canceled",
                                    text: $ele.data("confirm-no-message") || "Your Item is safe!",
                                    type: "error",
                                    timer: 2000
                                });
                            }
                        });
                    }
                });
            });
    },

    select2: function () {
        var $elements = $('[data-plugins*="select2"]');

        $.each($elements,
            function (i, ele) {
                var $ele = $(ele);

                $ele.select2({
                    placeholder: $ele.attr("placeholder")
                })
            });
    },

    tags: function () {
        var $elements = $('[data-plugins*="tags"]');

        $.each($elements,
            function (i, ele) {
                var $ele = $(ele);

                $ele.select2({
                    placeholder: $ele.attr("placeholder"),
                    tags: true,
                    multiple: true,
                    allowClear: true
                })
            });
    },

    clipboard: function () {
        var $elements = $('[data-plugins*="clipboard"]');

        $.each($elements,
            function (i, ele) {
                new Clipboard(ele).on("success", function (e) {
                    e.clearSelection();
                    global.util.notify("Copied !", e.text, "info");
                })
            });
    },

    datePicker: function () {
        var $elements = $('[data-plugins*="date-picker"]');

        $.each($elements,
            function (i, ele) {
                var $ele = $(ele);

                $ele.datepicker({
                    todayHighlight: true,
                    format: 'dd/mm/yyyy',
                    orientation: "bottom left",
                    templates: {
                        leftArrow: '<i class="la la-angle-left"></i>',
                        rightArrow: '<i class="la la-angle-right"></i>'
                    }
                });
            });
    },

    dropzone: function () {
        var $elements = $('[data-plugins*="dropzone"]');

        $.each($elements,
            function (i, ele) {
                var $ele = $(ele);

                var uploadedCallback = $ele.data("uploaded-callback");

                // Callback Element
                var callBackElement = $ele.data("dropzone-urls");
                var $callBackElement = $(callBackElement);

                // Max Files
                var maxFiles = $ele.data("max-file") || 1;

                // Preview
                $ele.addClass("m-dropzone dropzone");
                var innerHtml = $("#dropzone_template").html();
                $ele.append(innerHtml);
                $ele.find(".m-dropzone__msg-desc").html("Maximum is <strong> " + maxFiles + " </strong> images.");

                $ele.dropzone({
                    url: global.endpoint.api.image.uploadMultiPart,
                    paramName: "file",
                    maxFiles: maxFiles,
                    maxFilesize: $ele.data("max-file-size") || 5,
                    addRemoveLinks: true,
                    dictRemoveFile: "Delete",
                    dictCancelUpload: "Cancel",
                    previewTemplate: $("#dropzone_preview_template").html(),
                    init: function () {
                        // Initial

                        var $dropzone = this;

                        var prevFile;
                        var listFileInfo = [];

                        var urlsStr = $callBackElement.val();

                        if (urlsStr && urlsStr.length > 0) {
                            var urls = urlsStr.split(',');

                            $.each(urls, function (i, url) {
                                listFileInfo.push({
                                    uuid: url,
                                    url: url
                                });

                                var file = {
                                    name: url,
                                    accepted: true,
                                    kind: 'image',
                                    upload: {
                                        uuid: url
                                    }
                                };

                                $dropzone.files.push(file);

                                $dropzone.emit("addedfile", file);
                                $dropzone.emit("thumbnail", file, url);
                                $dropzone.emit("complete", file);
                            })
                        }

                        // Handle Upload
                        this.on('addedfile', function () {
                            if (typeof prevFile !== "undefined" && maxFiles === 1) {
                                this.removeFile(prevFile);
                            }
                        });

                        this.on('success', function (file, serverResponse) {
                            prevFile = file;

                            this.emit("thumbnail", file, serverResponse.url);

                            var fileInfo = {
                                uuid: file.upload.uuid,
                                url: serverResponse.url
                            };

                            if (listFileInfo && listFileInfo.length > 0 && maxFiles !== 1) {
                                listFileInfo.push(fileInfo);
                            } else {
                                listFileInfo = [fileInfo];
                            }

                            // Binding data to element
                            var urls = "";

                            $.each(listFileInfo, function (i, item) {
                                urls += "," + item.url;
                            });

                            urls = urls.substr(1);
                            $callBackElement.val(urls);

                            if (uploadedCallback) {
                                window[uploadedCallback](serverResponse);
                            }
                        });

                        this.on('removedfile', function (file) {
                            listFileInfo = listFileInfo.filter(function (value, index, arr) {
                                return value.uuid != file.upload.uuid;
                            });

                            // Binding data to element
                            var urls = "";
                            $.each(listFileInfo, function (i, item) {
                                urls += "," + item.url;
                            });
                            urls = urls.substr(1);
                            $callBackElement.val(urls);
                        });

                        this.on("error", function (file) {
                            this.removeFile(file);
                        });
                    }
                });
            });
    },

    init: function () {
        global.setup.ajax();
        global.setup.select2();
        global.setup.tags();
        global.setup.clipboard();
        global.setup.datePicker();
        global.setup.dropzone();
        global.setup.autoLoaderEle();
        global.setup.initSummerNote();
        global.setup.confirmPopup();
    }
};

global.util = {
    notify: function (title, message, type, options) {
        title = title || undefined;
        message = message || "";
        type = type || "primary";
        options = options || {};
        options.placement = options.placement || {};
        options.offset = options.offset || {};
        options.animate = options.animate || {};

        var content = {
            title: title,
            message: message
        };

        var alertType;

        switch (type) {
            case 'success':
                alertType = "success";
                break;
            case 'warning':
                alertType = "warning";
                break;
            case 'error':
                alertType = "danger";
                break;
            case 'info':
                alertType = "info";
                break;
            default:
                alertType = "primary";
        }

        var alert = $.notify(content, {
            type: alertType,
            allow_dismiss: options.allow_dismiss || true,
            newest_on_top: options.newest_on_top || false,
            mouse_over: options.mouse_over || true,
            showProgressbar: options.showProgressbar || false,
            spacing: options.spacing || "10",
            timer: options.timer || "5000",
            placement: {
                from: options.placement.from || "top",
                align: options.placement.align || "right"
            },
            offset: {
                x: options.offset.x || "30",
                y: options.offset.y || "30"
            },
            delay: options.delay || "1000",
            z_index: options.z_index || "10000",
            animate: {
                enter: options.animate.enter || 'animated bounce',
                exit: options.animate.exit || 'animated bounce'
            }
        });

        return alert;
    },

    loading: {
        show: function () {
            var $pageOverlay = $(".page-overlay");

            if ($pageOverlay && $pageOverlay.length > 0) {
                return;
            }

            mApp.blockPage({
                overlayColor: "#000000",
                type: "loader",
                state: "primary",
                message: "Processing..."
            });

            $pageOverlay = $("<div>");

            $pageOverlay.addClass("page-overlay");

            $('body').append($pageOverlay);


        },

        hide: function () {
            var $pageOverlay = $(".page-overlay");
            $pageOverlay.remove();
            mApp.unblockPage()
        }
    },

    dataTable: {
        imageRender: function (data, type, row) {
            if (data && data.trim().length > 0) {
                var eleId = global.util.guid.generate();

                var img = $("<img src='" + data + "' />");

                img.on('load', function (e) {
                    // Ignore
                }).on('error', function (e) {
                    // Cannot load image, remove or replace to default dead image
                    $("#" + eleId).attr("src", global.config.imageDeadUrl);
                });

                return '<div class="col-lg-12 text-center"><img class="dataTable-img" src="' + data + '" id="' + eleId + '"/></div>';
            }

            return "";
        }
    },

    guid: {
        generate: function () {
            return global.util.guid.s4() + global.util.guid.s4() + '-' + global.util.guid.s4() + '-' + global.util.guid.s4();
        },

        s4: function () {
            return Math.floor((1 + Math.random()) * 0x10000).toString(16).substring(1);
        },
    },

    openImage: function (ele) {
        var $ele = $(ele);

        var src = $ele.attr("src");

        window.open(src, '_blank').focus();
    }
};

$(function () {
    global.util.loading.hide();
    global.setup.init();
});
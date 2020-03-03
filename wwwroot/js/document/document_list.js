class DocumentList {
    // ID của document hiện đang select
    documentIDSelected = null;

    init() {
        //Binding event
        this.bindingEvent();
    }
    bindingEvent() {
        // sự kiện quay về folder cha
        $('.document-screen').on('click', '.btn-back', this.goToPreviousPage.bind(this));

        // sự kiện context menu
        $('.document-screen').on('contextmenu', '.document-container', this.showContextMenu.bind(this));

        // sự kiện click
        $('.document-screen').on('click', '.document-container', this.hideContextMenu.bind(this));

        // Xử lý sự kiện click item context menu
        $('.document-screen').on('click', '.menu-item', this.executeMenuItemClick.bind(this));

        // Xử lý event khi chọn file
        $('.document-screen').on('change', '#upload-input', this.handleInputUploadFileChange.bind(this));
    }

    goToPreviousPage(event) {
        history.go(-1);
    }

    showContextMenu(event) {
        var target = $(event.target);
        $('.document-context-menu .menu-item').hide();
        if (target.hasClass('document-container')) {
            $('.menu-item[action="add-folder"]').show();
            $('.menu-item[action="add-file"]').show();
        } else if (target.parents('.document').attr('type') == 'folder') {
            this.documentIDSelected = parseInt(target.parents('.document').attr('document-id'));
            $('.menu-item[action="open-folder"]').show();
            $('.menu-item[action="delete-document"]').show();
        } else {
            this.documentIDSelected = parseInt(target.parents('.document').attr('document-id'));
            $('.menu-item[action="dowload-file"]').show();
            $('.menu-item[action="delete-document"]').show();
        }
        event.preventDefault();
        var contextMenu = $('.document-context-menu');
        contextMenu.show();
        contextMenu.css({ "top": `${event.pageY}px`, "left": `${event.pageX}px` });
    }

    hideContextMenu() {
        var contextMenu = $('.document-context-menu');
        contextMenu.hide();
    }

    executeMenuItemClick(event) {
        var target = $(event.target);
        $('.document-context-menu').hide();
        switch (target.attr('action')) {
            case 'add-folder':
                this.openPopupAddFolder();
                break;
            case 'add-file':
                this.chooseFileUpload();
                break;
            case 'delete-document':
                this.deleteDocuemnt();
                break;
            default:
        }
    }

    openPopupAddFolder() {
        let me = this;
        let popup = new BasePopup();
        let config = {
            title: 'Thêm Folder',
            width: 500,
            buttons: [
                {
                    text: "Ok",
                    click: function () {
                        let formData = popup.getFormData();
                        // Hàm thêm new Folder
                        me.addNewFolder(formData);
                        $(this).dialog("close");
                    }
                },
                {
                    text: "Đóng",
                    click: function () {
                        $(this).dialog("close");
                    }
                }
            ]
        }
        popup.openPopup('document', 'AddFolder_Popup', config);
    }

    addNewFolder(formData) {
        let url = window.location.href;
        // Nếu phần cuối url không phải kiểu int thì đang ở root. lên gán parentID = 0
        let parentID = parseInt(url.split('/').pop()) ? parseInt(url.split('/').pop()) : 0;
        formData['parent_id'] = parentID;
        formData['type'] = 0
        $.ajax({
            url: 'https://localhost:44368/document/Insert',
            data: formData,
            method: 'POST',
            dataType: 'json',
            success: function (res) {
                if (res.success) {
                    location.reload();
                }
            }
        })
    }

    handleInputUploadFileChange(event) {
        let formData = new FormData();
        var files = event.target.files;
        for (var i = 0; i < files.length; i++) {
            formData.append('fileInput', files[i]);
        }
        let url = window.location.href;
        let parentID = parseInt(url.split('/').pop()) ? parseInt(url.split('/').pop()) : 0;
        formData.append('parentID', parentID);

        $.ajax({
            url: 'https://localhost:44368/FileHandle/UploadFile',
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function (res) {
                location.reload();
            }
        })
    }

    chooseFileUpload() {
        $(".document-screen #upload-input").click();
    }

    deleteDocuemnt() {
        $.ajax({
            url: `https://localhost:44368/document/Delete/${this.documentIDSelected}`,
            method: 'GET',
            dataType: 'json',
            success: function (res) {
                location.reload();
            }
        })
    }
}

var documentList = new DocumentList();
documentList.init();
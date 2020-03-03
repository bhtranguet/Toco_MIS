class BasePopup {
    selector = null;
    // khởi tạo popup
    openPopup(controller, viewName, config, data = null) {
        let me = this;
        $.ajax({
            url: `https://localhost:44368/${controller}/getView?viewName=${viewName}`,
            method: "GET",
            success: function (res) {
                me.selector = $(res);
                me.selector.dialog(config);
            }
        })
    }
    close() {
        this.selector.dialog("close");
    }
    getFormData() {
        var formData = {};
        let fields = this.selector.find('.field input');
        fields.each((index, field) => {
            let input = $(field);
            let fieldName = input.attr("name");
            let fieldValue = input.val();
            formData[fieldName] = fieldValue;
        })
        return formData;
    }
}
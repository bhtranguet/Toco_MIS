class PopupParam {
    constructor(controller, viewName, data = null, callback = null) {
        this.controller = controller;
        this.viewName = viewName;
        this.data = data;
        this.callback = callback;
    }
}
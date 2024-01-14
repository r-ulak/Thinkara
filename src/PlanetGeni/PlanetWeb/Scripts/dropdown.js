function DropDown(el) {
    this.dd = el;
    this.placeholder = this.dd.children('span');
    this.hiddenText = this.dd.children('input');
    this.opts = this.dd.find('ul.dropdown > li');
    this.val = '';
    this.index = -1;
    this.initEvents();

}


DropDown.prototype = {
    initEvents: function () {
        var obj = this;
        obj.dd.on('click', function (event) {
            $(this).toggleClass('active');
            return false;
        });
        obj.opts.on('click', function () {
            var opt = $(this);
            console.log(obj);
            obj.val = opt.text();
            obj.index = opt.index();
            obj.placeholder.text(obj.val);
            if (obj.hiddenText.length) {
                obj.hiddenText.val(obj.val);
            }

            obj.dd.data("id", opt.data("id"));
        });
    },
    getValue: function () {
        return this.val;
    },
    setValue: function (value, index, id) {
        var obj = this;
        console.log(obj);
        obj.val = value;
        obj.index = index;
        obj.placeholder.text(value);
        console.log("value is {0}", value);

        obj.dd.data("id", id);
    },
    getIndex: function () {
        return this.index;
    }
}
var excelHelper = {
    Sheetlength: 3,
    Columnlength: 10,
    RowLength: 5,
    DefaultStruct: function () {
        var struct = [];
        struct = new Array();
        for (var s = 0; s < excelHelper.Sheetlength; s++) {
            var sheet = excelHelper.InitSheet(s);
            struct.push(sheet);
        }
        return struct;
    },
    ConvertCode: function (code) {
        code = code.toUpperCase();
        var num = 0;
        num = 0;
        for (var i = code.length - 1, j = 1; i >= 0; i-- , j *= 26) {
            num += (code[i].charCodeAt() - 64) * j;
        }
        return num;
    },
    GenerateSheetCode: function (index) {
        return "Sheet" + (index + 1);
    },
    InitSheet: function (index) {
        var sheetCode = excelHelper.GenerateSheetCode(index);
        var sheet = {
            edit: false, code: sheetCode, sort: index, name: sheetCode, title: "", remark: "", firstrow: 3, firstcolumn: 1, changeflag: false, select: index == 0
        };
        sheet.columns = new Array();
        for (var i = 0; i < excelHelper.Columnlength; i++) {
            sheet.columns.push(excelHelper.InitColumn(i, index));
        }
        sheet.rows = new Array();
        for (var i = 0; i < excelHelper.RowLength; i++) {
            sheet.rows.push(excelHelper.InitRow(i, index));
        }
        return sheet;
    },
    InitCell: function (index, sheetindex, columnindex, rowindex) {
        return {
            ssort: sheetindex,
            rsort: rowindex,
            csort: columnindex,
            content: ''
        };
    },
    InitRow: function (index, sheetindex) {
        var cells = new Array();
        for (var j = 0; j < excelHelper.Columnlength; j++) {
            cells.push(excelHelper.InitCell(j, sheetindex, j, index));
        }
        return { cells: cells, sort: index };
    },
    InitColumn: function (index, sheetindex) {
        return { select: index == 0, sort: index, code: excelHelper.ParseCode(index + 1), name: "", type: 'Text', digit: 0, bgcolor: '255,255,255', required: false, range: '', isformula: false, tempformula: '', cellformula: '' };
    },
    ParseCode: function (i) {
        var s = "A B C D E F G H I J K L M N O P Q R S T U V W X Y Z";
        var sArray = s.split(" ");
        if (i < 1) return "";

        if (parseInt((i / 26) + "") == 0) return sArray[i % 26 - 1];
        else {
            if (i % 26 == 0) return (excelHelper.ParseCode(parseInt((i / 26) + "") - 1)) + sArray[26 - 1];
            else return sArray[parseInt((i / 26) + "") - 1] + sArray[i % 26 - 1];
        }
    },
    DefaultOptions: function () {
        return {
            //是否可添加sheet
            addsheet: true,
            //是否可删除sheet
            deletesheet: true,
            deletecolumn: true,
            renamesheet: true,
            //是否可维护sheet标题
            sheettitle: true,
            //是否可维护sheet填表备注
            sheetremark: true,
            //是否可添加列
            addcolumn: true,
            //是否可设计列单元格
            designcolumn: true,
            //是否可设计数据行单元格
            designdatarow: true,
            //是否可设计列定义
            designcolumndefine: true,
            //是否可设计数据区域
            designdatarea: false,
            designisrequired: true,
            viewremark: true
        };
    },
    DefaultReadOnlyOptions: function () {
        return {
            //是否可添加sheet
            addsheet: true,
            //是否可删除sheet
            deletesheet: true,
            deletecolumn: true,
            renamesheet: true,
            //是否可维护sheet标题
            sheettitle: true,
            //是否可维护sheet填表备注
            sheetremark: true,
            //是否可添加列
            addcolumn: true,
            //是否可设计列单元格
            designcolumn: true,
            //是否可设计数据行单元格
            designdatarow: true,
            //是否可设计列定义
            designcolumndefine: true,
            //是否可设计数据区域
            designdatarea: false,
            designisrequired: true,
            viewremark: true
        };
    },
    ImportOptions: function () {
        return {
            //是否可添加sheet
            addsheet: false,
            //是否可删除sheet
            deletesheet: true,
            deletecolumn: false,
            renamesheet: false,
            //是否可维护sheet标题
            sheettitle: false,
            //是否可维护sheet填表备注
            sheetremark: false,
            //是否可添加列
            addcolumn: false,
            //是否可设计列单元格
            designcolumn: false,
            //是否可设计数据行单元格
            designdatarow: false,
            //是否可设计列定义
            designcolumndefine: false,
            //是否可设计数据区域
            designdatarea: false,
            designisrequired: true,
            viewremark: false
        };
    },
    ImportReadOnlyOptions: function () {
        return {
            //是否可添加sheet
            addsheet: false,
            //是否可删除sheet
            deletesheet: true,
            deletecolumn: false,
            renamesheet: false,
            //是否可维护sheet标题
            sheettitle: false,
            //是否可维护sheet填表备注
            sheetremark: false,
            //是否可添加列
            addcolumn: false,
            //是否可设计列单元格
            designcolumn: false,
            //是否可设计数据行单元格
            designdatarow: false,
            //是否可设计列定义
            designcolumndefine: false,
            //是否可设计数据区域
            designdatarea: false,
            designisrequired: true,
            viewremark: false
        };
    },
    LoadOptions: function (renderType, Edit) {
        var options = null;
        switch (renderType) {
            case "0":
                {
                    options = Edit ? excelHelper.DefaultReadOnlyOptions() : excelHelper.DefaultOptions();
                }
                break;
            case "1":
                {
                    options = Edit ? excelHelper.DefaultReadOnlyOptions() : excelHelper.DefaultOptions();
                }
                break;
            case "2":
                {
                    options = Edit ? excelHelper.ImportReadOnlyOptions() : excelHelper.ImportOptions();
                }
                break;
        }
        return options;
    }

};

Array.prototype.ExistByCode = function (code) {
    var ret = false;
    for (var i = 0; i < this.length; i++) {
        if (this[i].code == code) {
            ret = true;
            break;
        }
    }
    return ret;
}
Array.prototype.RemoveByCode = function (code) {

    for (var i = 0; i < this.length; i++) {
        if (this[i].code == code) {
            this.splice(i, 1);
            break;
        }
    }

}
Array.prototype.IndexOfByCode = function (code) {
    var index = -1;
    for (var i = 0; i < this.length; i++) {
        if (this[i].code == code) {
            index = i;
            break;
        }
    }
    return index;
}
Array.prototype.SelectByCode = function (code) {
    var ret = null;
    for (var i = 0; i < this.length; i++) {
        if (this[i].code == code) {
            ret = this[i];
            break;
        }
    }
    return ret;
}
Array.prototype.Exist = function (value) {
    var ret = false;
    for (var i = 0; i < this.length; i++) {
        if (this[i] == value) {
            ret = true;
            break;
        }
    }
    return ret;
}
Vue.component('vue-excel', {
    props: ["sheets", "options"],
    template: "#ucExcel",
    data: function () {
        var self = this;
        return {
            excel: self.sheets,
            options: self.options,
            isActive: true,
            sort: ""
        };
    },
    created: function () {
        var self = this;
        self.$on("excel-remove-sheet", function (sheet) {
            self.removeSheet(sheet);
        });
        this.sortSheet();
    },
    updated: function () {
    },
    destoryed: function () {
    },
    methods: {
        compareSheet: function (source, target) {
            return source.code == target.code;
        },
        tabSheet: function (sheet) {
            var self = this;
            for (var i = 0; i < self.sheets.length; i++) {
                var item = self.sheets[i];
                item.select = self.compareSheet(item, sheet);
            }
        },
        removeSheet: function (sheet) {
            var self = this;
            utils.confirmMessage("是否确定进行该操作？", function (ret) {
                if (!ret) {
                    return;
                }
                if (self.sheets.length == 1) {
                    utils.alertMessage("至少应保留一个工作表", function () {
                    });
                    return;
                }
                var select = false;
                for (var i = 0; i < self.sheets.length; i++) {
                    var item = self.sheets[i];
                    var compare = self.compareSheet(item, sheet);
                    if (compare) {
                        select = item.select;
                        self.sheets.splice(i, 1);
                    }
                }
                //如果原sheet是被选中状态  重新设置第一个为选中
                if (select) {
                    self.sheets[0].select = true;
                }
            });
        },
        addSheet: function () {
            var self = this;
            var index = 0;
            while (true) {
                var code = "Sheet" + (index + 1);
                if (!self.sheets.ExistByCode(code)) {
                    break;
                }
                index++;
            }
            var sheet = excelHelper.InitSheet(index);
            sheet.select = false;
            self.sheets.push(sheet);
        },
        editSheet: function (sheet) {
            sheet.edit = true;
        },
        appendColumn: function (sheet) {
            for (var i = 0; i < excelHelper.Columnlength; i++) {
                var column = excelHelper.InitColumn(sheet.columns.length, 0);
                sheet.columns.push(column);
                for (var j = 0; j < sheet.rows.length; j++) {
                    sheet.rows[j].cells.push(excelHelper.InitCell(j, 0, i, j));
                }
            }
        },
        removeColumn: function (sheet, column) {
            utils.confirmMessage("是否确定进行删除？", function (ret) {
                if (!ret) {
                    return;
                }
                if (sheet.columns.length == 1) {
                    utils.alertMessage("请至少保留一列", function () {

                    });
                    return;
                }
                var columnIndex = 0;
                for (var i = 0; i < sheet.columns.length; i++) {
                    var item = sheet.columns[i];
                    if (item.code == column.code) {
                        sheet.columns.splice(i, 1);
                        columnIndex = i;
                        break;
                    }
                }
                for (var c = 0; c < sheet.rows.length; c++) {
                    var row = sheet.rows[c];
                    for (var i = 0; i < row.cells.length; i++) {
                        if (i == columnIndex) {
                            row.cells.splice(i, 1);
                        }
                    }
                }
            })

        },
        defineColumn: function (sheet, column) {
            //designcolumndefine 是否可设计列 如果不可以 则文本框都是只读 但是否必填特殊 可以考虑不该属性而designisrequired
            for (var i = 0; i < sheet.columns.length; i++) {
                var item = sheet.columns[i];
                item.select = (column.code == item.code);
            }
        },
        doRenameSheet: function (sheet, $event) {
            var self = this;
            //utils.confirmMessage("是否确定进行该操作", function (ret) {
            //    if (!ret) {
            //        return;
            //    }
            var v = $event.target.value;
            if (v.length == 0) {
                utils.alertMessage("请输入名称后再进行保存");
                return;
            }
            for (var i = 0; i < self.sheets.length; i++) {
                if (v == self.sheets[i].name && sheet.code != self.sheets[i].code) {
                    utils.alertMessage("名称重复");
                    return;
                }
            }
            sheet.name = v;
            sheet.edit = false;
            sheet.changeflag = true;
            //});
        },
        undoRenameSheet: function (sheet) {
            sheet.edit = false;
        },
        sortSheet: function () {
            this.excel.forEach(function (one) {
                one.startSort = -1;
                one.endSort = -1
            })
        },
        tabActive: function (type) {
            if (type == "code") {
                this.isActive = true;
            } else {
                this.isActive = false;
            }
        },
        sortChange: function (sort) {
            this.sort = sort;
        },
        setSort: function () {

        },
        checkedRequireFilter: function (sheet) {
            var str = ""
            sheet.columns.forEach(function (one) {
                if (one.required) {
                    str += one.code + ","
                }
            })
            return str.slice(0, str.length - 1);
        }
    },
    watch: {
        sheets: function () {
            var self = this;
            self.sortSheet();
        }
    },
    computed: {
    },
    updated: function () {
        var self = this;
        for (var i = 0; i < self.sheets.length; i++) {
            var sheet = self.sheets[i];
            for (var j = 0; j < sheet.columns.length; j++) {
                sheet.columns[j].code = excelHelper.ParseCode(j + 1);
                sheet.rangePos = "";
            }
        }
    }
});
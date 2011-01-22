var store = null;

function initData()
{
    var Employee = Ext.data.Record.create([{
        name: 'name',
        type: 'string'
    }, {
        name: 'email',
        type: 'string'
    }, {
        name: 'start',
        type: 'date',
        dateFormat: 'n/j/Y'
    },{
        name: 'salary',
        type: 'float'
    },{
        name: 'active',
        type: 'bool'
    }]);

    var genData = function(){
        var data = [];
        var s = new Date(2007, 0, 1);
        var now = new Date(), i = -1;
        while(s.getTime() < now.getTime()){
            var ecount = 3;
            for(var i = 0; i < ecount; i++){
                var name = "test";
                data.push({
                    start : s.clearTime(true).add(Date.DAY, 27),
                    name : name,
                    email: name.toLowerCase().replace(' ', '.') + '@exttest.com',
                    active: true,
                    salary: Math.floor(85000/1000)*1000
                });
            }
            s = s.add(Date.MONTH, 1);
        }
        return data;
    }

    store = new Ext.data.GroupingStore({
        reader: new Ext.data.JsonReader({fields: Employee}),
        data: genData(),
        sortInfo: {field: 'start', direction: 'ASC'}
    });
}
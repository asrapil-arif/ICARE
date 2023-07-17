function find_contract_type(Target,TargetId){

        w2popup.message({ 
        width   : 700, 
        height  : 350,
        body    : '<div style="padding: 0px;padding-bottom:0px;"><div id="ListContract" style="height:300px;border:none;border-right:none;border-bottom:1px dotted #dedede;border-radius:0px;" class="responsive"></div></div>',
        onOpen : function (event) {

                $('#ListContract').w2grid({ 
                    name: 'ListContract', 
                    multiSelect     : false,
                    header: 'Contract Type',
                    textSearch: 'contains',
                    show: {
                    header         : true,
                    toolbar     : true,
                    footer        : true,
                    lineNumbers    : true,
                    toolbarSearch     : false,
                    }, 
                    searches: [
                    { field: 'Id', caption: 'Id', type: 'text' },
                    { field: 'ContractType', caption: 'Name', type: 'text' },

                    ],
                    columns: [                
                    { field: 'Id', caption: 'Id', size: '100px',sortable: true },
                    { field: 'ContractType', caption: 'Name', size: '300px',sortable: true },
                    ],
                    onReload: function (event) {
                    refresh_contract_type();
                    },
                    onDblClick: function (event) {
                                select_contract(Target,TargetId);
                        }
                });

                refresh_contract_type();
        },
        onClose : function (event) {
                w2ui['ListContract'].destroy();
        } ,
        buttons : '<button class="w2ui-btn" onclick="select_contract(\''+Target+'\',\'' + TargetId + '\')">Select</button>'+
                    '<button class="w2ui-btn" onclick="w2popup.message()">Cancel</button>' ,
        });
}

function refresh_contract_type(){

w2ui['ListContract'].load(URL_PTR + '/Contract/ContractTypeList');

}

function select_contract(TargetName,TargetId){

    var sel = w2ui.ListContract.getSelection() ;
    var record  = w2ui.ListContract.get(sel[0]);
    $("#"+ TargetId).val(record.Id);
    $("#"+ TargetName).val(record.ContractType);
    $("#"+ TargetId).trigger("change");
    w2popup.message();

}


function ListVendor_(Target,TargetId){

    w2popup.message({ 
    width   : 700, 
    height  : 350,
    body    : '<div style="padding: 0px;padding-bottom:0px;"><div id="ListVendor" style="height:300px;border-left:none;border-bottom:1px dotted #dedede;border-radius:0px;" class="responsive"></div></div>',
    onOpen : function (event) {

            $('#ListVendor').w2grid({ 
                name: 'ListVendor', 
                multiSelect     : false,
                header: 'List Vendor',
                textSearch: 'contains',
                show: {
                header         : true,
                toolbar     : true,
                footer        : true,
                lineNumbers    : true,
                }, 
                searches: [
                { field: 'VendorId', caption: 'Id.', type: 'text' },
                { field: 'VendorName', caption: 'Nama Vendor', type: 'text' },
                { field: 'VendorType', caption: 'Tipe', type: 'text' },
          
                ],
                columns: [                
                { field: 'VendorId', caption: 'Id.', size: '50px',sortable: true },
                { field: 'VendorName', caption: 'Nama Vendor', size: '300px',sortable: true },
                { field: 'VendorType', caption: 'Tipe', size: '100px',sortable: true },
           

                ],
                onReload: function (event) {
                refresh_list_vendor();
                },
                onDblClick: function (event) {
                           select_vendor_list(Target,TargetId);
                 }
            });
       
            refresh_list_vendor();
        
    },
    onClose : function (event) {
          w2ui['ListVendor'].destroy();
    } ,
    buttons : '<button class="w2ui-btn" onclick="select_vendor_list(\''+Target+'\',\'' + TargetId + '\')">Select</button>'+
                '<button class="w2ui-btn" onclick="w2popup.message()">Cancel</button>' ,
    });
}

function refresh_list_vendor(){

w2ui['ListVendor'].load(URL_PTR + '/Contract/ListVendor');

}

function select_vendor_list(TargetName,TargetId){

    var sel = w2ui.ListVendor.getSelection() ;
    var record  = w2ui.ListVendor.get(sel[0]);
    $("#"+ TargetId).val(record.VendorId);
    $("#"+ TargetName).val(record.VendorName);
    $("#"+ TargetName).trigger("change");
    w2popup.message();

}



function ListGroup_(TargetId,TargetName){

    w2popup.message({ 
    width   : 400, 
    height  : 350,
    body    : '<div style="padding: 0px;padding-bottom:0px;"><div id="ListGroup" style="height:300px;border-left:none;border-bottom:1px dotted #dedede;border-radius:0px;" class="responsive"></div></div>',
    onOpen : function (event) {

            $('#ListGroup').w2grid({ 
                name: 'ListGroup', 
                multiSelect     : false,
                header: 'List Group Item',
                textSearch: 'contains',
                show: {
                header         : true,
                toolbar     : true,
                footer        : true,
                lineNumbers    : true,
                }, 
                columns: [                
                { field: 'GroupId', caption: 'ID', size: '20px',sortable: true },
                { field: 'GroupName', caption: 'Name', size: '240px',sortable: true },
                ],
                onReload: function (event) {
                refresh_list_group_item();
                },
                onDblClick: function (event) {
                           select_group_item(TargetId,TargetName);
                 }
            });
       
            refresh_list_group_item();
        
    },
    onClose : function (event) {
          w2ui['ListGroup'].destroy();
    } ,
    buttons : '<button class="w2ui-btn" onclick="select_group_item(\''+TargetId+'\',\'' + TargetName + '\')">Select</button>'+
                '<button class="w2ui-btn" onclick="w2popup.message()">Cancel</button>' ,
    });


}

function refresh_list_group_item(){
    w2ui['ListGroup'].load(URL_PTR + '/MasterData/MasterGroupItemList');
}

function select_group_item(TargetId,TargetName){

    var sel = w2ui.ListGroup.getSelection();
    var record  = w2ui.ListGroup.get(sel[0]);
    $("#"+ TargetId).val(record.GroupId);
    $("#"+ TargetName).val(record.GroupName);
    $("#"+ TargetName).trigger("change");

    w2popup.message();

}


function ListPengguna_(Target,TargetId,Posisi,Fungsi){
    w2popup.message({ 
    width   : 700, 
    height  : 350,
    body    : '<div style="padding: 0px;padding-bottom:0px;"><div id="ListPengguna" style="height:300px;border-left:none;border-bottom:1px dotted #dedede;border-radius:0px;" class="responsive"></div></div>',
    onOpen : function (event) {

            $('#ListPengguna').w2grid({ 
                name: 'ListPengguna', 
                multiSelect     : false,
                header: 'List Pengguna',
                textSearch: 'contains',
                show: {
                header         : true,
                toolbar     : true,
                footer        : true,
                lineNumbers    : true,
                }, 
                searches: [
                { field: 'KodePengguna', caption: 'Nip', type: 'text' },
                { field: 'NamaPengguna', caption: 'Nama', type: 'text' },  
                { field: 'Posisi', caption: 'Posisi', type: 'text' },  
                { field: 'Fungsi', caption: 'Fungsi', type: 'text' },   
                ],
                columns: [                
                { field: 'KodePengguna', caption: 'Nip', size: '100px',sortable: true },
                { field: 'NamaPengguna', caption: 'Nama', size: '240px',sortable: true },
                { field: 'Posisi', caption: 'Posisi', size: '240px',sortable: true },
                { field: 'Fungsi', caption: 'Fungsi', size: '240px',sortable: true } //
                ],
                onReload: function (event) {
                refresh_list_pengguna();
                },
                onDblClick: function (event) {
                           select_pengguna_mutasi(Target,TargetId,Posisi,Fungsi);
                 }
            });
       
            refresh_list_pengguna();
        
    },
    onClose : function (event) {
          w2ui['ListPengguna'].destroy();
    } ,
    buttons : '<button class="w2ui-btn" onclick="select_pengguna_mutasi(\''+Target+'\',\'' + TargetId + '\',\''+ Posisi + '\',\''+ Fungsi + '\')">Select</button>'+
                '<button class="w2ui-btn" onclick="w2popup.message()">Cancel</button>' ,
    });
}

function refresh_list_pengguna(){

w2ui['ListPengguna'].load(URL_PTR + '/MasterData/ListPengguna');


}

function select_pengguna_mutasi(TargetName,TargetId,Posisi,Fungsi){

    var sel = w2ui.ListPengguna.getSelection();
    var record  = w2ui.ListPengguna.get(sel[0]);
    $("#"+ TargetName).val(record.NamaPengguna);
    $("#"+ TargetId).val(record.KodePengguna);

    $("#"+ TargetName).trigger("change");

    if (Posisi != null || Posisi != ""){
    
        $("#"+ Posisi).val(record.Posisi);
    }

    if (Fungsi != null || Fungsi != ""){
    
        $("#"+ Fungsi).val(record.Fungsi);
    }
    w2popup.message();

}


function ListUnit_(Target,TargetID){


    w2popup.message({ 
    width   : 700, 
    height  : 350,
    body    : '<div style="padding: 0px;padding-bottom:0px;"><div id="ListUnit" style="height:300px;border-left:none;border-bottom:1px dotted #dedede;border-radius:0px;" class="responsive"></div></div>',
    onOpen : function (event) {

            $('#ListUnit').w2grid({ 
                name: 'ListUnit', 
                multiSelect     : false,
                header: 'Location List',
                textSearch: 'contains',
                show: {
                toolbar     : true,
                header         : true,
                footer        : true,
                lineNumbers    : true,
                }, 
                columns: [                
                { field: 'KodeUnit', caption: 'Id.', size: '100px',sortable: true ,searchable: 'text' },
                { field: 'NamaUnit', caption: 'Name', size: '240px',sortable: true ,searchable: 'text' },
                { field: 'TipeUnit', caption: 'Category', size: '140px',sortable: true ,searchable: 'text'},
                { field: 'JenisUnit', caption: 'Type', size: '100px',sortable: true ,searchable: 'text' },
       
                ],
                onReload: function (event) {
                refresh_unit_list();
                },
                onDblClick: function (event) {
                           select_unit_mutasi(Target,TargetID);
                 }
            });
       
            refresh_unit_list();
        
    },
    onClose : function (event) {
          w2ui['ListUnit'].destroy();
    } ,
    buttons : '<button class="w2ui-btn" onclick="select_unit_mutasi(\''+Target+'\',\''+ TargetID + '\')">Select</button>'+
                '<button class="w2ui-btn" onclick="w2popup.message()">Cancel</button>' ,
    });




}

function refresh_unit_list(){

w2ui['ListUnit'].load(URL_PTR + '/MasterData/ListUnit');


}

function select_unit_mutasi(TargetName,TargetId){

    var sel = w2ui.ListUnit.getSelection()
    var record  = w2ui.ListUnit.get(sel[0]);
    $("#"+ TargetName).val(record.NamaUnit);
    $("#"+ TargetId).val(record.KodeUnit);
    $("#"+ TargetName).trigger("change");
    $("#"+ TargetId).trigger("change");
    w2popup.message();

}



function ListJenisPengajuan_(TargetName,TargetId){
    w2popup.message({ 
    width   : 700, 
    height  : 350,
    body    : '<div style="padding: 0px;padding-bottom:0px;"><div id="ListGroupPengajuan" style="height:300px;border-left:none;border-bottom:1px dotted #dedede;border-radius:0px;" class="responsive"></div></div>',
    onOpen : function (event) {

            $('#ListGroupPengajuan').w2grid({ 
                name: 'ListGroupPengajuan', 
                multiSelect     : false,
                header: 'Pilih Group Pengajuan',
                textSearch: 'contains',
                show: {
                header         : true,
                toolbar     : true,
                footer        : true,
                lineNumbers    : true,
                toolbarSearch : false,
                }, 
                columns: [                
                { field: 'GroupId', caption: 'ID', size: '100px',sortable: true,searchable:true },
                { field: 'GroupName', caption: 'Group', size: '240px',sortable: true,searchable:true }
                ],
                onReload: function (event) {
                refresh_list_jenis_peng();
                },
                onDblClick: function (event) {
                           select_jenis_pengajuan(TargetName,TargetId);
                 }
            });
       
            refresh_list_jenis_peng();
        
    },
    onClose : function (event) {
          w2ui['ListGroupPengajuan'].destroy();
    } ,
    buttons : '<button class="w2ui-btn" onclick="select_jenis_pengajuan(\''+TargetName+'\',\'' + TargetId + '\')">Select</button>'+
                '<button class="w2ui-btn" onclick="w2popup.message()">Cancel</button>' ,
    });
}

function refresh_list_jenis_peng(){

w2ui['ListGroupPengajuan'].load(URL_PTR + '/Request/ListGroup');


}

function select_jenis_pengajuan(TargetName,TargetId){

    var sel = w2ui.ListGroupPengajuan.getSelection();
    var record  = w2ui.ListGroupPengajuan.get(sel[0]);
    $("#"+ TargetName).val(record.GroupName);
    $("#"+ TargetId).val(record.GroupId);

    $("#"+ TargetName).trigger("change");

    w2popup.message();

}



function list_assets(record,myGrid){

    w2popup.message({ 
    width   : 900, 
    height  : 420,
    body    : '<div style="padding: 0px;padding-bottom:0px;"><div id="ListAssets" style="height:370px;border-left:none;border-bottom:1px dotted #dedede;border-radius:0px;" class="responsive"></div></div>',
    onOpen : function (event) {

            $('#ListAssets').w2grid({ 
                name: 'ListAssets', 
                multiSelect     : true,
                header: 'Pilih Asset Untuk Realisasi',
                textSearch: 'contains',
                show: {
                header         : true,
                toolbar     : true,
                footer        : true,
                lineNumbers    : true,
                toolbarSearch : true,
                toolbarInput : false,
                }, 
                columns: [               
                { field: 'AssetId', caption: 'ID', size: '30px',sortable: true,searchable:true,frozen: true },
                { field: 'NamaAsset', caption: 'Name', size: '100px',sortable: true,searchable:true ,frozen: true },
                { field: 'NamaKategori', caption: 'Kategori', size: '100px',sortable: true,searchable:true },
                { field: 'ServiceTag', caption: 'Servive Tag', size: '100px',sortable: true,searchable:true },
                { field: 'NoSeri', caption: 'No.Seri', size: '100px',sortable: true,searchable:true },
                { field: 'KodeLokasi', caption: 'Lokasi', size: '100px',sortable: true,searchable:true },
                { field: 'KodeUnit', caption: 'Unit', size: '100px',sortable: true,searchable:true },
                { field: 'NamaUnit', caption: 'Nama Unit', size: '100px',sortable: true,searchable:true },
                { field: 'NamaFungsi', caption: 'Fungsi', size: '100px',sortable: true,searchable:true },
                { field: 'NamaPengguna', caption: 'Pengguna', size: '100px',sortable: true,searchable:true },
                ],
                onReload: function (event) {
                refresh_list_assets();
                },
                onDblClick: function (event) {
                           select_asset(record.recid,myGrid);
                 }
            });
       
            refresh_list_assets();
        
    },
    onClose : function (event) {
          w2ui['ListAssets'].destroy();
    } ,
    buttons : '<button class="w2ui-btn" onclick="select_asset(\''+record.recid+'\',\'' + myGrid + '\')">Select</button>'+
                '<button class="w2ui-btn" onclick="w2popup.message()">Cancel</button>' ,
    });
}

function refresh_list_assets(){

w2ui['ListAssets'].load(URL_PTR + '/Request/ListAssetData');

}

function select_asset(Id,myGrid){

    var sel = w2ui.ListAssets.getSelection();
    var record  = w2ui.ListAssets.get(sel[0]);
    w2ui[myGrid].set(Id, { RealisasiId: record.AssetId,NamaAsset :record.NamaAsset});

    w2popup.message();

}
/*
* jQuery Mobile v1.4.5
* http://jquerymobile.com
*
* Copyright 2010, 2014 jQuery Foundation, Inc. and other contributors
* Released under the MIT license.
* http://jquery.org/license
* extend by asrapil arif 2022 Juli
*/
if ("undefined" == typeof jQuery) throw new Error("Ace's JavaScript requires jQuery");
(function ($) {

			$.fn.multi_attachment_button = function (options, method,x) {
				options = $.extend({}, $.fn.multi_attachment_button.options, options);
				var clean_id = this[0].id.replace("#", ""),
					id = this[0].id;

				var settings = $.extend({
					upload_array_name: null,
					deleting_process: null,
					uploading_process: null
				}, options);


				var button = '<button type="button" class="btn btn-tools-attachment btn-white btn-attach-info">...</button>';
				button = button + '<button type="button" class="btn btn-tools-attachment btn-white btn-clear-attach" hidden ><i class="ace-icon fa fa-trash" aria-hidden="true"></i> Clear Attachment File</button>';
				button = button + '<button type="button" class="btn btn-tools-attachment btn-white btn_add_attachment" onclick="$(\'#' + id + '\').val(\'\');$(\'#' + id + '\').click();"><i class="ace-icon fa fa-paperclip" aria-hidden="true" style="color:orange;"></i> Tambahkan Attachment</button>';
				button = button + '<div class="btn-tools-attachment uploading_progress" style="color:dodgerblue;border:none !important;"><img src="../Content/images/loading_images/save_loading_cloud.gif" style="width:20px;padding:0px !important;" /> Uploading Please wait ...</div>';

				var container_id = clean_id + '_container_multiple_upload';
				var container_div = '<div id="' + container_id + '">' + button + '</div>'

				if (method == undefined) {

					$("#" + id).before(container_div);
					$("#" + id).appendTo("#" + container_id);
					$("#" + id).hide();
					$("#" + container_id + " .btn").hide();
					$("#" + container_id + " .btn_add_attachment").show();
					$("#" + container_id + " .file_controll").hide();
					$("#" + container_id + " .uploading_progress").hide();
				}

				var Methods = {
					/**
					 * Default method.
					 */
					init: function () {
						alert('init');
					},

					/**
					 * Destroy plugin.
					 */
					upload_complete: function () {

						$("#" + container_id + " .btn-clear-attach").show();
						$("#" + container_id + " .btn-attach-info").html(eval(x).length + " File Attached");
						$("#" + container_id + " .btn-attach-info").show();
					}

				};

				if (Methods[method]) {
					return Methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
				} else if (typeof options === 'object' || !options) {
					// default to "init"
					//return Methods.init.apply(this, arguments);
				} else {
					$.error('Method ' + method + ' does not exist.');
				}

				$("#" + container_id + " .btn-clear-attach").click(function () {
					try {
						if (settings.deleting_process != null) {

							settings.deleting_process();
							$("#" + container_id + " .btn-clear-attach").hide();
							$("#" + container_id + " .uploading_progress").hide();
							$("#" + container_id + " .btn-attach-info").html("");
							$("#" + container_id + " .btn-attach-info").hide();

						}
					}
					catch (err) {
						alert(err.message);
					}
				})
				return this;
			};
})(jQuery);


//cara penggunaan

/*
	$("#attachment_contract").multi_attachment_button({
		upload_array_name: "attachment_tools_contract_file",
		deleting_process: function () {

			attachment_tools_contract_file = [];
		}
	}).on("change", function () {

		var file = this.files[0];
		var file_name = file.name;
		var DataSave = new FormData();
		DataSave.append('FileCategory', "CONTRACT_FILE");
		DataSave.append('UPLOAD_FILE', file);
		DataSave.append('Key1', 'CONTRACT_FILE');
		DataSave.append('Key2', '0');



		$("#attachment_contract_container_multiple_upload .uploading_progress").show("fade", function () {

			var Simpan = $.ajax({
				url: URL_PTPR + '/EContract/UploadFilePre',
				type: 'POST',
				data: DataSave,
				dataType: "json",
				cache: false,
				async: false,
				contentType: false,
				processData: false,
				error: function (request, status, error) {
					alert("Upload File Gagal !");
					$("#attachment_contract_container_multiple_upload .uploading_progress").hide();
				}
			});

			if (Simpan.responseJSON["Result"].toLocaleLowerCase().indexOf("error") != -1) {
				alert("Upload File Gagal !");
				$("#attachment_contract_container_multiple_upload .uploading_progress").hide();
			}
			else {
				var row_upload = { ID: Simpan.responseJSON["Result"] };
				attachment_tools_contract_file.push(row_upload);
				alert("Upload File Berhasil !");
				$("#attachment_contract_container_multiple_upload .uploading_progress").hide();
				$("#attachment_contract").multi_attachment_button(null, "upload_complete", "attachment_tools_contract_file");
			}
		});


	})
	*/

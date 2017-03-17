var http = require('http');
var mysql = require('mysql');
var bodyParser = require('body-parser');
var express = require('express');
var app = express();
var async = require("async");
var fs = require('fs');
var dbLogic=require('./dbLogic');

fs.readFile('units.json', 'utf8', function (err, contents) {
	var obj = JSON.parse(contents);
	async.each(obj, function (unit, callback) {
		var csvData = (fs.readFileSync(unit.csv, 'utf8'));
		csvData = csvData.replace(/Element\;samarbeidsrom\/supenr\/Lists\/Skiftrapport Bod/g, '').replace(/Element;samarbeidsrom\/supenr\/Lists\/rapos/g, '').replace(/\;/g, '').replace(/[\r|\r\n]/g, "\n").replace(/\"/g, '').replace(/(Skiftrapport)|(Skiftrapport )|(skiftrapport)/g, '').replace(/\.2015/g, '\.15').replace(/\.2014/g, '\.14');
		csvData = csvData.replace("'", " ");
		if (csvData.indexOf("'") > -1) {
			console.log("Error here");

		}
		csvData = csvData.split(/([0-9][0-9]\.[0-9][0-9]\.[0-9][0-9])/g);

		var logg = {},
			hide = true;

		//  $("#log-" + $scope.superVisorLogID).empty();
		//   jQuery('#logContainer-' + $scope.superVisorLogID).show();

		for (i = 0; i < csvData.length; i++) {
			i % 2 === 0 ? 0 : logg[csvData[i]] = csvData[i + 1];
		}
		var logs = [];
		for (var key in logg) {
			var pieces = key.split('.');
			var temp = pieces[2];
			pieces[2] = '20' + pieces[2];
			pieces.reverse();
			var date = pieces.join('-');
			logs.push({
				logdate: date,
				logdata: logg[key],
				unitid: unit.id
			});
		}
		async.each(logs, function (supervisorlog, callback) {

			// Perform operation on file here.

			var query = "call spSaveSupervisorLogs('" + supervisorlog.logdate + "','" + supervisorlog.logdata + "'," + supervisorlog.unitid + ")";

			connection.query(query, function (err, recordset) {
				if (err) {
					console.log(err);
				}
				else {
					callback();
				}
			})

		}, function (err) {
			// if any of the file processing produced an error, err would equal that error
			if (err) {
				// One of the iterations produced an error.
				// All processing will now stop.
				console.log('A log failed to process');
			} else {
				console.log('success');
			}
		});

		callback();

	}, function (err) {
		// if any of the file processing produced an error, err would equal that error
		if (err) {
			// One of the iterations produced an error.
			// All processing will now stop.
			console.log('A log failed to process');
		} else {
			console.log('success');
			fs.writeFile("units.json", "[]", function (err) {
				if (err) {
					return console.log(err);
				}
			});
		}
	});
});

/* create MySQL connection parameters */
var connection = mysql.createConnection({
    host: '192.168.1.75',
    database: "argus",
    user: "root",
    password: "root",
    port: 3306
});

app.use(bodyParser.json());
app.use(bodyParser.urlencoded({
    // to support URL-encoded bodies
    extended: true
}));

/* connect to MySQL database */
connection.connect(function (err) {
    if (err) {
        console.log('Error connecting to Db');
        return;
    }
    console.log('Connection established');
});

app.use(function (req, res, next) { //allow cross origin requests
    res.setHeader("Access-Control-Allow-Methods", "POST, PUT, OPTIONS, DELETE, GET");
    res.header("Access-Control-Allow-Origin", "*");
    res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With,authorization, Content-Type, Accept");
    next();
});

/* get reports */
app.get('/getReports', function (request, response) {
    /* call this method to get all reports */
    dbLogic.getReports(function (result) {
        /* if error in query result then return error result otherwise return success result */
        if (!result.success) {
            response.send("error"); /* send response with error message */
        }
        else {
            response.send(result.message[0]); /* send query result to client */
        }
    });
})

/* get report versions */
app.get('/getReportVersions', function (request, response) {

    /* call this method to get all report versions */
    dbLogic.getReportVersions(function (result) {
        /* if error in query result then return error result otherwise return success result */
        if (!result.success) {
            response.send("error"); /* send response with error message */
        }
        else {
            response.send(result.message); /* send query result to client */
        }
    });
})
app.get('/getLordShiva', function (request, response) {
    var obj={
        Lord:'Shiva',
        Enemy:'Hemant'

    }
  response.send(obj);
})
/* get report plugins */
app.get('/getReportPlugins', function (request, response) {

    var reportid = request.query.reportid;                  // get reportod from request query
    var reportversionid = request.query.reportversionid;    // get reportversionid from request query
    var isNew = request.query.isNew;                        // get isNew from request query
    console.log("Request coming for report version" + request.query.reportversionid)
    if (isNew == 'true' || isNew == true) {
        if (reportid == "0" && reportversionid == "0") {
            saveReportAndReportVersion(function (result) {
                /* if error in query result then return error result otherwise return success result */
                if (!result.success) {
                    response.send("error"); /* send response with error message */
                }
                else {
                    response.send(result.message); /* send query result to client */
                }
            })
        }
        else {
            response.send("success"); /* send response with success message */
        }
    }
    else {
        if (reportid == "0" && reportversionid == "0") {
            /* call this method to get max report version id and reportid */
            dbLogic.getMaxReportVersion(function (result) {
                /* if error in query result then return error result otherwise continue for other process */
                if (!result.success) {
                    response.send("error"); /* send response with error message */
                }
                else {
                    var message = result.message[0];
                    if (message[0] == undefined) {
                        reportid = undefined;
                        reportversionid = undefined;
                    }
                    else {
                        reportid = message[0].reportId;
                        reportversionid = message[0].id;
                    }
                    if (reportid == undefined && reportversionid == undefined) {
                        saveReportAndReportVersion(function (result) {
                            /* if error in query result then return error result otherwise return success result */
                            if (!result.success) {
                                response.send("error"); /* send response with error message */
                            }
                            else {

                                response.send(result.message); /* send query result to client */
                            }
                        })
                    }
                    else {
                        /* call getReportPlugins method to get report plugins according to reportversionid */
                        dbLogic.getReportPlugins(reportversionid, function (result) {
                            /* if error in query result then return error result otherwise return success result */
                            if (!result.success) {
                                response.send("error"); /* send response with error message */
                            }
                            else {
                                var obj = { reportId: reportid, reportVersionId: reportversionid, reportPlugins: result.message[0], isDeleted: false };
                                response.send(obj); /* send query result to client */
                            }
                        });
                    }
                }
            })
        }
        else {

            var query = "select * from reportversion where id=" + reportversionid;

            executeQuery(query, function (result) {
                if (!result.success) {
                    response.send("error"); /* send response with error message */
                }
                else {
                    var isdeleted = true;
                    if (result.message[0] != undefined) {
                        isdeleted = result.message[0].isdeleted;
                    }

                    /* call getReportPlugins method to get report plugins according to reportversionid */
                    dbLogic.getReportPlugins(reportversionid, function (result) {
                        /* if error in query result then return error result otherwise return success result */
                        if (!result.success) {
                            response.send("error"); /* send response with error message */
                        }
                        else {
                            var obj = { reportId: reportid, reportVersionId: reportversionid, reportPlugins: result.message[0], isDeleted: isdeleted };
                            response.send(obj); /* send query result to client */
                        }
                    });
                }
            })
        }
    }
})

/* save report version */
app.post('/saveReportVersion', function (request, response) {

    var data = request.body;

    var reportid = data.reportid;
    var reportversionid = data.reportversionid;

    /* call getReportPlugins method to get report plugins according to reportversionid */
    dbLogic.getReportPlugins(reportversionid, function (result) {
        /* if error in query result then return error result otherwise continue for other process */
        if (!result.success) {
            response.send("error"); /* send response with error message */
        }
        else {
            var reportplugins = result.message[0];

            /* call this method to get maxReportVersionNo according to report */
            dbLogic.getMaxReportVersionNo(reportid, function (result) {
                /* if error in query result then return error result otherwise continue for other process */
                if (!result.success) {
                    response.send("error"); /* send response with error message */
                }
                else {
                    var newVersionNo = parseInt(result.message[0].maxversion) + 1;

                    dbLogic.saveReportVersion(0, reportid, "", newVersionNo, function (result) {
                        /* if error in query result then return error result otherwise continue for other process */
                        if (!result.success) {
                            response.send("error"); /* send response with error message */
                        }
                        else {
                            var j = 0;
                            var obj = { reportId: reportid, reportVersionId: result.message[0][0].ID };
                            var titlePlugin = [];
                            for (var i = 0; i < reportplugins.length; i++) {
                                // reportplugins[i].id= Date.parse(new Date()) + i;

                                if (reportplugins[i].pluginTypeId == 8) {
                                    titlePlugin = reportplugins[i];
                                }
                                /* call saveReportPlugin method to save reportplugins */
                                dbLogic.saveReportPlugin(Date.parse(new Date()) + i, reportplugins[i].pluginTypeId, reportplugins[i].sort, JSON.parse(reportplugins[i].parameterValue), obj.reportVersionId,0, function (result) {
                                    /* if error in query result then return error result otherwise continue for other process */
                                    if (!result.success) {
                                        response.send("error"); /* send response with error message */
                                    }
                                    else {
                                        if (j == reportplugins.length - 1) {
                                            var parameter = JSON.parse(titlePlugin.parameterValue);
                                            /* call saveReportVersion method to update title in report version */
                                            dbLogic.saveReportVersion(obj.reportVersionId, reportid, parameter.title, "", function (result) {
                                                if (!result.success) {
                                                    response.send("error"); /* send response with error message */
                                                }
                                                else {
                                                    /* call saveNewVersionPluginChartData to save new plugin chart data */
                                                    saveNewVersionPluginChartData(obj.reportVersionId, reportplugins, function (result) {
                                                        /* if error in query result then return error result otherwise return success result */
                                                        if (!result.success) {
                                                            response.send("error"); /* send response with error message */
                                                        }
                                                        else {
                                                            response.send(obj); /* send query result to client */
                                                        }
                                                    })
                                                }
                                            });
                                        }
                                        j++;
                                    }
                                })
                            }
                        }
                    })
                }
            })
        }
    })
})

/* save plugin parameter detail and also save plugin sort in database */
app.post('/saveReportPluginDetails', function (request, response) {
    var data = request.body;
    
    if (data.editType == 'single') {
        var query = "select * from versionedit where reportversionid=" + data.reportVersionId + " order by id desc limit 1";
        
        executeQuery(query, function (result) {
            /* if error in query result then return error result otherwise continue for other process */
            if (!result.success) {
                response.send("error"); /* send response with error message */
            }
            else {
                var lastVersionEdit = result.message[0];
                
                if (lastVersionEdit == undefined) {
                    /* call getReportPluginByID method to get report plugin according to reportpluginid */
                    dbLogic.getReportPluginByID(data.id, function (result) {
                        /* if error in query result then return error result otherwise continue for other process */
                        if (!result.message) {
                            response.send("error"); /* send response with error message */
                        }
                        else {
                            var lastReportPlugin = result.message[0];
                            if(lastReportPlugin)
                                lastReportPlugin=0;
                            /* call saveReportPluginAndVersionEdits method to save version edit 
                               and save and update report plugin and update version title */
                            saveReportPluginAndVersionEdits(data, [], lastReportPlugin, function (result) {
                                /* if error in query result then return error result otherwise return success result */
                                if (!result.success) {
                                    response.send("error"); /* send response with error message */
                                }
                                else {
                                    response.send(result.message[0][0]); /* send query result to client */
                                }
                            })
                        }
                    })
                }
                else {
                    /* call getVersionEditContent method to get edit contents according to versioneditid */
                    dbLogic.getVersionEditContent(lastVersionEdit.id, function (result) {
                        /* if error in query result then return error result otherwise continue for other process */
                        if (!result.success) {
                            response.send("error"); /* send response with error message */
                        }
                        else {
                            var editContent = result.message[0][0];
                            
                            /* call getReportPluginByID method to get report plugin according to reportpluginid */
                            dbLogic.getReportPluginByID(data.id, function (result) {
                                /* if error in query result then return error result otherwise continue for other process */
                                if (!result.message) {
                                    response.send("error"); /* send response with error message */
                                }
                                else {
                                    var lastReportPlugin = result.message[0];
                                    if (lastVersionEdit.type == 'single') {
                                        if (editContent != undefined) {
                                            if (editContent.parametervalue == lastReportPlugin.parameterValue) {
                                                /* call saveReportPluginDetail method to save and update report plugin and update version title */
                                                saveReportPluginDetail(data, function (result) {
                                                    /* if error in query result then return error result otherwise return success result */
                                                    if (!result.success) {
                                                        response.send("error"); /* send response with error message */
                                                    }
                                                    else {
                                                        response.send(result.message[0][0]); /* send query result to client */
                                                    }
                                                })
                                            }
                                            else {
                                                /* call saveReportPluginAndVersionEdits method to save version edit 
                                               and save and update report plugin and update version title */
                                                saveReportPluginAndVersionEdits(data, [], lastReportPlugin, function (result) {
                                                    /* if error in query result then return error result otherwise return success result */
                                                    if (!result.success) {
                                                        response.send("error"); /* send response with error message */
                                                    }
                                                    else {
                                                        response.send(result.message[0][0]); /* send query result to client */
                                                    }
                                                })
                                            }
                                        }
                                        else {
                                            /* call saveReportPluginAndVersionEdits method to save version edit 
                                               and save and update report plugin and update version title */
                                            saveReportPluginAndVersionEdits(data, [], lastReportPlugin, function (result) {
                                                /* if error in query result then return error result otherwise return success result */
                                                if (!result.success) {
                                                    response.send("error"); /* send response with error message */
                                                }
                                                else {
                                                    response.send(result.message[0][0]); /* send query result to client */
                                                }
                                            })
                                        }
                                    }
                                    else {
                                        /* call saveReportPluginAndVersionEdits method to save version edit 
                                           and save and update report plugin and update version title */
                                        saveReportPluginAndVersionEdits(data, [], lastReportPlugin, function (result) {
                                            /* if error in query result then return error result otherwise return success result */
                                            if (!result.success) {
                                                response.send("error"); /* send response with error message */
                                            }
                                            else {
                                                response.send(result.message[0][0]); /* send query result to client */
                                            }
                                        })
                                    }
                                }
                            })
                        }
                    });
                }
            }
        })
    }
    else if (data.editType == 'multiple') {
        /* call getReportPlugins method to get report plugins according to reportversionid */
        dbLogic.getReportPlugins(data.reportVersionId, function (result) {
            /* if error in query result then return error result otherwise continue for other process */
            if (!result.success) {
                response.send("error"); /* send response with error message */
            }
            else {
                var reportPlugins = result.message[0];

                /* call saveReportPluginAndVersionEdits method to save version edit 
                   and save and update report plugin and update version title */
                saveReportPluginAndVersionEdits(data, reportPlugins, {}, function (result) {
                    /* if error in query result then return error result otherwise return success result */
                    if (!result.success) {
                        response.send("error"); /* send response with error message */
                    }
                    else {
                        response.send(result.message[0][0]); /* send query result to client */
                    }
                })
            }
        })
    }
    else {
        /* call saveReportPluginDetail method to save and update report plugin and update version title */
        saveReportPluginDetail(data, function (result) {
            /* if error in query result then return error result otherwise return success result */
            if (!result.success) {
                response.send("error"); /* send response with error message */
            }
            else {
                response.send(result.message[0][0]); /* send query result to client */
            }
        })
    }
})

/* create api method to delete Report Plugin from database */
app.delete('/deleteReportPlugin', function (request, response) {
    var reportPluginId = request.query.reportPluginId; // get reportpluginid from request query

    /* create and execute query to get reportversionid by reportpluginid */
    var query = "select reportVersionId from reportplugin where id=" + reportPluginId;

    executeQuery(query, function (result) {
        /* if error in query result then return error result otherwise continue for other process */
        if (!result.success) {
            response.send("error"); /* send response with error message */
        }
        else {
            var reportVersionId = result.message[0].reportVersionId;
            /* call this method to delete and save version edits */
            saveVersionEditsInDeleteAndSortingCase(reportVersionId, function (result) {
                /* if error in query result then return error result otherwise continue for other process */
                if (!result.success) {
                    response.send("error"); /* send response with error message */
                }
                else {
                    /* call this method to delete report plugin */
                    dbLogic.deleteReportPlugin(reportPluginId, function (result) {
                        /* if error in query result then return error result otherwise return success result */
                        if (!result.success) {
                            response.send("error"); /* send response with error message */
                        }
                        else {
                            response.send(result.message); /* send query result to client */
                        }
                    })
                }
            });
        }
    })
})

/* create api method to update report plugin sorting in database */
app.post('/updateReportPluginSorting', function (request, response) {

    /* get data from request */
    var data = request.body.pluginsOrder;               // get pluginOrders from request
    var isVersionEdit = request.body.isVersionEdit;     // get isVersionEdit from request
    var reportversionid = request.body.reportversionid; // get reportversionid from request

    if (isVersionEdit == 'true' || isVersionEdit == true) {
        /* call this method to delete and save version edits */
        saveVersionEditsInDeleteAndSortingCase(reportversionid, function (result) {
            /* if error in query result then return error result otherwise continue for other process */
            if (!result.success) {
                response.send("error"); /* send response with error message */
            }
            else {
                var j = 0;
                /* update all plugin sorting in database */
                for (var i = 0; i < data.length; i++) {
                    /* call updateReportPlugin method to update plugin sorting */
                    dbLogic.updateReportPlugin(data[i].id, 'sort', data[i].sort, '', 0, function (result) {
                        /* if error in query result then return error result otherwise return success result */
                        if (!result.success) {
                            response.send("error"); /* send response with error message */
                        }
                        else {
                            if (j == data.length - 1) {
                                response.send("success"); /* send success result to client */
                            }
                        }
                        j++;
                    })
                }
            }
        })
    }
    else {
        var j = 0;
        /* update all plugin sorting in database */
        for (var i = 0; i < data.length; i++) {
            /* call updateReportPlugin method to update plugin sorting */
            dbLogic.updateReportPlugin(data[i].id, 'sort', data[i].sort, '', 0, function (result) {
                /* if error in query result then return error result otherwise return success result */
                if (!result.success) {
                    response.send("error"); /* send response with error message */
                }
                else {
                    if (j == data.length - 1) {
                        response.send("success"); /* send success result to client */
                    }
                }
                j++;
            })
        }
    }
})

/* create api method to update report plugin values in database */
app.post('/updateReportPluginValues', function (request, response) {

    /* get data from request */
    var data = request.body;

    /* call updateReportPlugin method to update report plugin parameter */
    dbLogic.updateReportPlugin(data.id, data.columnName, 0, data.parameters, data.isMinimized, function (result) {
        /* if error in query result then return error result otherwise return success result */
        if (!result.success) {
            response.send("error"); /* send response with error message */
        }
        else {
            response.send("success"); /* send response with success message */
        }
    })
})

app.get('/getXMl', function (request, response) {
var object='<note><to>Tove</to><from>Jani</from></note>'
response.header('Content-Type','text/xml').send(object)
})

/* create api method to get supervisor logs from database */
app.post('/getSupervisorLogs', function (request, response) {


    var logdates = [];
    for (var i = 0; i < request.body.dates.length; i++) {
        logdates.push("'" + request.body.dates[i] + "'")
    }

    var ids = logdates.join();

    /* if logdates not found then return logdates and end response of request */
    if (logdates.length == 0) {
        response.send(logdates); /* send query result to client */
    }
    else {

        /* create query for get supervisor log according to logdate and unit */
        var query = "select * from supervisorlog  WHERE logdate IN (" + ids + ") and unitid=" + request.body.unitId;
        /* if logdate not found then create query for get supervisor log according to unit */
        if (ids == "") {
            query = "select * from supervisorlog  WHERE  unitid=" + request.body.unitid;
        }

        /* execute query to get supervisor log from database */
        executeQuery(query, function (result) {
            /* if error in query result then return error result otherwise return success result */
            if (!result.success) {
                response.send("error"); /* send response with error message */
            }
            else {
                response.send(result.message); /* send query result to client */
            }
        })
    }
})

/* create api method to get version edits from database */
app.get('/getVersionEdits', function (request, response) {

    var reportversionid = request.query.reportversionid; // get reportversionid from request query

    /* create and execute query to get version edits according to reportversionid */
    var query = "call spGetVersionEdits(" + reportversionid + ")";

    executeQuery(query, function (result) {
        /* if error in query result then return error result otherwise return success result */
        if (!result.success) {
            response.send("error"); /* send response with error message */
        }
        else {
            response.send(result.message[0]); /* send query result to client */
        }
    })
})

/* create api method to restore version edit content in report plugin database */
app.get('/restoreVersionEdit', function (request, response) {

    var versionEditId = request.query.versionEditId;     // get versioneditid from request query
    var edittype = request.query.edittype;               // get edittype from request query
    var reportversionid = request.query.reportversionid; // get reportversionid from request query
    var reportid = request.query.reportid;               // get reportid from request query

    /* call getVersionEditContent method to get edit contents according to versioneditid */
    dbLogic.getVersionEditContent(versionEditId, function (result) {
        /* if error in query result then return error result otherwise continue for other process */
        if (!result.success) {
            response.send("error"); /* send response with error message */
        }
        else {
            var editPlugins = result.message[0]; // version edit plugins
            /* create and execute query to get max plugin sort from report plugin according to reportversionid */
            var query = "select MAX(sort) sort from reportplugin where reportversionid=" + reportversionid + " and pluginTypeId!=8";
            executeQuery(query, function (result) {
                /* if error in query result then return error result otherwise continue for other process */
                if (!result.success) {
                    response.send("error"); /* send response with error message */
                }
                else {
                    var maxSort = -1;
                    if (result.message[0].sort != null && result.message[0].sort != undefined) {
                        maxSort = parseInt(result.message[0].sort);
                    }
                    /* call getReportPlugins method to get report plugins according to reportversionid */
                    dbLogic.getReportPlugins(reportversionid, function (result) {
                        /* if error in query result then return error result otherwise continue for other process */
                        if (!result.success) {
                            response.send("error"); /* send response with error message */
                        }
                        else {
                            var reportPlugins = result.message[0];
                            /* if edittype is "single" then save single plugin in version edit otherwise save all plugins in versionedit database */
                            if (edittype == 'single') {

                                /* if edit not found in report plugin then deletedReportPluginId is zero
                                   and it will insert new record in report plugin otherwise
                                   update plugin content in report plugin */

                                if (editPlugins[0].deletedReportPluginId == 0) {
                                    var title = '';
                                    editPlugins[0].reportpluginid = 0;
                                    /* if plugin is not title plugin then get report plugin id from plugin parameters */
                                    if (editPlugins[0].pluginTypeId != 8) {
                                        for (var i = 0; i < reportPlugins.length; i++) {
                                            if (reportPlugins[i].pluginTypeId == editPlugins[0].pluginTypeId) {
                                                if (editPlugins[0].pluginTypeId == 5) {
                                                    if (editPlugins[0].parametervalue == reportPlugins[i].parametervalue) {
                                                        editPlugins[0].reportpluginid = reportPlugins[i].id;
                                                        break;
                                                    }
                                                }
                                                else {
                                                    var parameter = JSON.parse(reportPlugins[i].parametervalue);
                                                    var param = JSON.parse(editPlugins[0].parametervalue);
                                                    if (parameter.id == param.id) {
                                                        editPlugins[0].reportpluginid = reportPlugins[i].id;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    /* create object to save report plugin detail */
                                    var data = { id: editPlugins[0].reportpluginid, pluginTypeId: editPlugins[0].pluginTypeId, sort: maxSort + 1, parametervalue: editPlugins[0].parametervalue, reportversionid: reportversionid, reportid: reportid, title: title };

                                    /* call saveReportPluginDetail method to save and update report plugin and update version title */
                                    saveReportPluginDetail(data, function (result) {
                                        /* if error in query result then return error result otherwise return success result */
                                        if (!result.success) {
                                            response.send("error"); /* send response with error message */
                                        }
                                        else {
                                            response.send("success"); /* send success result to client */
                                        }
                                    })
                                }
                                else {
                                    /* call getReportPluginByID method to get report plugin according to reportpluginid */
                                    dbLogic.getReportPluginByID(editPlugins[0].reportpluginid, function (result) {
                                        /* if error in query result then return error result otherwise continue for other process */
                                        if (!result.success) {
                                            response.send("error"); /* send response with error message */
                                        }
                                        else {
                                            var reportPlugin = result.message[0];
                                            /* create object to save version edit */
                                            var data = { reportversionid: reportversionid, edittype: edittype };
                                            /* call delete and save version edits method */
                                            deleteAndSaveVersionEdits(data, [], reportPlugin, function (result) {
                                                /* if error in query result then return error result otherwise continue for other process */
                                                if (!result.success) {
                                                    response.send("error"); /* send response with error message */
                                                }
                                                else {
                                                    var title = '';
                                                    /* if plugin is title plugin get title from plugin parameter */
                                                    if (editPlugins[0].pluginTypeId == 8) {
                                                        var parameter = JSON.parse(editPlugins[0].parametervalue);
                                                        title = parameter.title;
                                                    }

                                                    /* create object to save report plugin detail */
                                                    data = { id: editPlugins[0].reportpluginid, pluginTypeId: editPlugins[0].pluginTypeId, sort: editPlugins[0].sort, parametervalue: editPlugins[0].parametervalue, reportversionid: reportversionid, reportid: reportid, title: title };

                                                    /* call saveReportPluginDetail method to save and update report plugin and update version title */
                                                    saveReportPluginDetail(data, function (result) {
                                                        /* if error in query result then return error result otherwise return success result */
                                                        if (!result.success) {
                                                            response.send("error"); /* send response with error message */
                                                        }
                                                        else {
                                                            response.send("success"); /* send success result to client */
                                                        }
                                                    })
                                                }
                                            })
                                        }
                                    })
                                }
                            }
                            else {
                                /* create object to save version edit */
                                var data = { reportversionid: reportversionid, edittype: edittype };
                                /* call delete and save version Edits method */
                                deleteAndSaveVersionEdits(data, reportPlugins, {}, function (result) {
                                    /* if error in query result then return error result otherwise continue for other process */
                                    if (!result.success) {
                                        response.send("error"); /* send response with error message */
                                    }
                                    else {
                                        var j = 0;

                                        /* restore version edit plugins in report plugin */
                                        for (var i = 0; i < editPlugins.length; i++) {

                                            var title = '';
                                            /* if plugin is title plugin then get title from report parameter to update title of report version */
                                            if (editPlugins[i].pluginTypeId == 8) {
                                                var parameter = JSON.parse(editPlugins[i].parametervalue);
                                                title = parameter.title;
                                            }

                                            /* if edit not found in report plugin then deletedReportPluginId is zero
                                               and it will insert new record in report plugin otherwise
                                               update plugin content in report plugin */

                                            if (editPlugins[i].deletedReportPluginId == 0) {
                                                maxSort = maxSort + 1; // create max sort

                                                editPlugins[i].reportpluginid = 0;

                                                /* if plugin is not title plugin then get report plugin id from plugin parameters */
                                                if (editPlugins[i].pluginTypeId != 8) {
                                                    for (var j = 0; j < reportPlugins.length; j++) {
                                                        if (reportPlugins[j].pluginTypeId == editPlugins[i].pluginTypeId) {
                                                            if (editPlugins[i].pluginTypeId == 5) {
                                                                if (editPlugins[i].parametervalue == reportPlugins[j].parametervalue) {
                                                                    editPlugins[i].reportpluginid = reportPlugins[j].id;
                                                                    break;
                                                                }
                                                            }
                                                            else {
                                                                var rptParameter = JSON.parse(reportPlugins[j].parametervalue);
                                                                var param = JSON.parse(editPlugins[i].parametervalue);
                                                                if (rptParameter.id == param.id) {
                                                                    editPlugins[i].reportpluginid = reportPlugins[j].id;
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                /* create object to save report plugin detail */
                                                data = { id: editPlugins[i].reportpluginid, pluginTypeId: editPlugins[i].pluginTypeId, sort: maxSort, parametervalue: editPlugins[i].parametervalue, reportversionid: reportversionid, reportid: reportid, title: title };
                                            }
                                            else {
                                                /* create object to save report plugin detail */
                                                data = { id: editPlugins[i].reportpluginid, pluginTypeId: editPlugins[i].pluginTypeId, sort: editPlugins[i].sort, parametervalue: editPlugins[i].parametervalue, reportversionid: reportversionid, reportid: reportid, title: title };
                                            }

                                            /* call saveReportPluginDetail method to save and update report plugin and update version title */
                                            saveReportPluginDetail(data, function (result) {
                                                /* if error in query result then return error result otherwise return success result */
                                                if (!result.success) {
                                                    response.send("error"); /* send response with error message */
                                                }
                                                else {
                                                    if (j == editPlugins.length - 1) {
                                                        response.send("success"); /* send success result to client */
                                                    }
                                                    j++;
                                                }
                                            })
                                        }
                                    }
                                })
                            }

                        }
                    });
                }
            });
        }
    })
})

/* create api method to save and update plugin chart data in database */
app.post('/savePluginChartData', function (request, response) {

    /* get data from request */
    var data = request.body;

    /* create and execute query to get pluginchartid according to reportpluginid */
    var query = "select id from pluginchart where reportpluginid=" + data.reportpluginid;

    executeQuery(query, function (result) {
        if (!result.success) {
            response.send("error"); /* send response with error message */
        }
        else {
            var id = data.id;
            /* if id not found in database then set id to zero */
            if (result.message[0] == undefined) {
                id = 0;
            }
            else {
                id = result.message[0].id;
            }

            /* call savePluginChart method to save or update data in pluginchart table in database */
            dbLogic.savePluginChart(data.chartdata, data.reportpluginid, function (result) {
                /* if error in query result then return error result otherwise return success result */
                if (!result.success) {
                    response.send("error"); /* send response with error message */
                }
                else {
                    response.send(result.message[0][0]); /* send query result to client */
                }
            })
        }
    })
});

/* create api method to get line chart data from database */
app.get('/getLineChartData', function (request, response) {

    var lineChartId = request.query.lineChartId;            // get linechartid from request query
    var reportVersionId = request.query.reportVersionId;    // get reportversionid from request query
    var pluginTypeId = request.query.pluginTypeId;                  // get pluginTypeId from request query

    /* call this method to get reportplugins according to reportversionid */
    dbLogic.getReportPlugins(reportVersionId, function (result) {
        if (!result.success) {
            response.send("error"); /* send response with error message */
        }
        else {
            var reportPlugins = result.message[0];

            reportPlugins = reportPlugins.filter(function (item) {
                return item.pluginTypeId == pluginTypeId;
            })

            for (var i = 0; i < reportPlugins.length; i++) {

                var parameter = JSON.parse(reportPlugins[i].parameterValue);

                if (parameter.id == lineChartId) {
                    /* create and execute query to get line chart data */
                    var query = "select * from pluginchart where reportpluginid=" + reportPlugins[i].id;

                    executeQuery(query, function (result) {
                        if (!result.success) {
                            response.send("error"); /* send response with error message */
                        }
                        else {
                            response.send(result.message[0]); /* send query result to client */
                        }
                    })
                }
            }
        }
    })
})

/* create api method to delete report or report version from database */
app.delete('/deleteReportOrVersion', function (request, response) {

    /* get data from request */
    var data = request.body;

    var reportid = data.reportId;
    var reportversionid = data.reportVersionId;
    var action = data.action;

    var query = "select * from reportversion where reportid = " + reportid + " and isdeleted = false order by id";

    executeQuery(query, function (result) {
        if (!result.success) {
            response.send("error"); /* send response with error message */
        }
        else {
            var reportVersions = result.message;
            var returnObj = { isNew: true, reportid: 0, reportversionid: 0 };

            if (action == 'deleteReport' || (action == 'deleteReportVersion' && reportVersions.length == 1)) {
                query = "call spDeleteReportAndVersions(" + reportid + ")";
                executeQuery(query, function (result) {
                    if (!result.success) {
                        response.send("error"); /* send response with error message */
                    }
                    else {
                        response.send(returnObj); /* send query result to client */
                    }
                })
            }
            else if (action == 'deleteReportVersion') {
                query = "update reportversion set isdeleted = true where id=" + reportversionid;
                //query = "delete from reportversion where id=" + reportversionid;
                executeQuery(query, function (result) {
                    if (!result.success) {
                        response.send("error"); /* send response with error message */
                    }
                    else {
                        var found = false;
                        returnObj.isNew = false;
                        returnObj.reportid = reportid;

                        //for (var i = 0; i < reportVersions.length; i++) {
                        //    if (i == 0 && reportVersions[i].id == reportversionid) {
                        //        found = true;
                        //    }
                        //    if (i == 1 && found == true) {
                        //        returnObj.reportversionid = reportVersions[i].id;
                        //        break;
                        //    }
                        //    if (i > 0 && reportVersions[i].id == reportversionid) {
                        //        break;
                        //    }
                        //    returnObj.reportversionid = reportVersions[i].id;
                        //}

                        reportVersions = reportVersions.filter(function (item) {
                            return item.id != reportversionid;
                        });

                        // a and b are object elements of your array
                        reportVersions.sort(function (a, b) {
                            return parseInt(a.versionno) - parseInt(b.versionno);
                        });

                        var lastVersion = reportVersions[reportVersions.length - 1];
                        returnObj.reportversionid = lastVersion.id;

                        response.send(returnObj); /* send query result to client */
                    }
                })
            }
        }
    })
});

/* get Demandchart data from reportplugin table */
app.get('/getDemandChartData', function (request, response) {

    var data = request.query;
    var reportPluginId = data.reportPluginId;

    /* create and execute query to get all report versions */
    var query = "select * from reportplugin where id=" + reportPluginId;

    executeQuery(query, function (result) {
        if (!result.success) {
            response.send("error"); /* send response with error message */
        } else {
            response.send(result.message[0]); /* send query result to client */
        }
    })
})

/* get chart data by dates from database */
app.get('/getChartData', function (request, response) {

    var data = request.query;

    var dates = data.dates;
    var unitId = data.unitId;
    var intervalMinutes = parseInt(data.interval);
    var includeFlaws = data.flaws == 'true' ? true : false;

    /* create and execute query to get all sector logs according to dates and calendar unit */
    var query = "select sl.id, sl.sector, DATE_FORMAT(sl.date, '%Y-%m-%d') date, sl.weekday, sl.logblob, sl.starttimeminutes, sl.durationminutes " +
                "from sectorlog sl, sector s " +
                "where sl.date IN (" + dates + ") " +
                "AND sl.sector = s.id AND s.sectorgroup = " + unitId + " " +
                "AND ((" + data.includeEC + " = s.isec AND s.isec = 1) OR (" + data.includePC + " = s.ispc AND s.ispc = 1))";

    executeQuery(query, function (result) {
        if (!result.success) {
            response.send("error"); /* send response with error message */
        }
        else {
            var sectorLogs = result.message;

            var obj = { sectorLogs: sectorLogs, chartHours: [], isLineChart: data.isLineChart };

            if (data.isLineChart == true || data.isLineChart == 'true') {

                query = "select sum(sl.durationminutes)/60 as sumHours, DATE_FORMAT(sl.date, '%Y-%m-%d') date " +
                        "from sectorlog sl, sector s " +
                        "where sl.date IN (" + dates + ") " +
                        "and sl.sector = s.id and s.sectorgroup = " + unitId + " " +
                        "group by sl.date";

                executeQuery(query, function (result) {
                    if (!result.success) {
                        response.send("error"); /* send response with error message */
                    }
                    else {
                        var chartHours = result.message;

                        obj.chartHours = chartHours;

                        response.send(obj);      /* send query result to client */
                    }
                })
            }
            else {
                response.send(obj);      /* send query result to client */
            }
        }
    })
})

/* currently this method is not use */
function deletePluginAndEdits(reportversionid, callback) {
    deleteVersionEdits(reportversionid, 0, function (result) {
        /* if error occurred during database communication then
           it will return error result otherwise it will continue for further process */
        if (!result.success) {
            callback(result); /* return callback result */
        }
        else {

            /* call getReportPlugins method to get report plugins according to reportversionid */
            dbLogic.getReportPlugins(reportversionid, function (result) {
                /* if error in query result then return error result otherwise return success result */
                if (!result.success) {
                    callback(result); /* return callback result */
                }
                else {
                    var reportPlugins = result.message[0];
                    var j = 0;
                    for (var i = 0; i < reportPlugins.length; i++) {
                        dbLogic.deleteReportPlugin(reportPlugins[i].id, function (result) {
                            if (j == reportPlugins.length - 1) {
                                callback(result); /* return callback result */
                            }
                            j++;
                        })
                    }
                }
            });
        }
    });
}

/* create method to call other method to save report and report version and get report plugins */
function saveReportAndReportVersion(callback) {

    /* call saveReport method to save report */
    dbLogic.saveReport('admin', function (result) {
        /* if error occurred during database communication then
           it will return error result otherwise it will continue for further process */
        if (!result.success) {
            callback(result);
        }
        else {      
                  
            /* get reportid to save reportversion */
            var reportId = result.message[0][0].ID;            
            /* call saveReportVersion method to save report version */
            dbLogic.saveReportVersion(0, reportId, "", 1, function (result) {
                /* if error occurred during database communication then
                   it will return error result otherwise it will continue for further process */
                if (!result.success) {
                    callback(result);            
                }
                else {
                    /* get reportversionid to get report plugins */
                    var reportVersionId = result.message[0][0].ID;
                    /* call getReportPlugins method to get report plugins according to reportversionid */
                    dbLogic.getReportPlugins(reportVersionId, function (result) {
                        /* if error occurred during database communication then
                           it will return error result otherwise it will return success result */
                        if (!result.success) {
                            callback(result);
                        }
                        else {
                            var obj = { reportId: reportId, reportVersionId: reportVersionId, reportPlugins: result.message[0], isDeleted: false };
                            var obj2 = { success: result.success, message: obj };
                            callback(obj2); /* return callback result */
                        }
                    });
                }
            })
        }
    })
}

/* create function to call other methods */
function saveReportPluginAndVersionEdits(data, reportPlugins, lastReportPlugin, callback) {
    /* call delete and save version edits method */
    deleteAndSaveVersionEdits(data, reportPlugins, lastReportPlugin, function (result) {
        /* if error occurred during database communication then 
           it will return error result otherwise it will continue for further process */
        if (!result.success) {
            callback(result); 
        }
        else {
            /* call saveReportPluginDetail method to save and update report plugin and update version title */
            saveReportPluginDetail(data, function (result) {
                callback(result); /* return callback result */
            })
        }
    })
}

/* create function to save and update report plugin with title of report version */
function saveReportPluginDetail(data, callback) {
    /* call save report plugin method */
    dbLogic.saveReportPlugin(data.id, data.pluginTypeId, data.sort, data.parameterValue, data.reportVersionId,data.isMinimized, function (result) {
        /* if error occurred during database communication then 
           it will return error result otherwise it will continue for further process */
        if (!result.success) {
            callback(result); 
        }
        else {
            var pluginData = result;
            if (data.pluginTypeId == 8) {
                /* call saveReportVersion method to update title in report version */
                dbLogic.saveReportVersion(data.reportVersionId, data.reportId, data.title, "", function (result) {
                    callback(pluginData); /* return callback result */
                })
            }
            else {
                callback(pluginData); /* return callback result */
            }
        }
    })
}

/* create function to save and update version edits in only when user delete plugin and change sorting of plugin */
function saveVersionEditsInDeleteAndSortingCase(reportVersionId, callback) {
    /* call get report plugins method */
    dbLogic.getReportPlugins(reportVersionId, function (result) {
        /* if error occurred during database communication then
           it will return error result otherwise it will continue for further process */
        if (!result.success) {
            callback(result);
        }
        else {
            var reportPlugins = result.message[0];

            var editdata = { reportVersionId: reportVersionId, editType: 'multiple' };
            /* call delete and save version edit method */
            deleteAndSaveVersionEdits(editdata, reportPlugins, {}, function (result) {
                callback(result); /* return callback result */
            })
        }
    })
}


/* create function to delete old and insert new version edit */
function deleteAndSaveVersionEdits(data, reportPlugins, lastReportPlugin, callback) {

    /* call saveVersionEdits method to save latest version edit */
    saveVersionEdits(data, reportPlugins, lastReportPlugin, function (result) {
        if (!result.success) {
            callback(result); /* return callback result */
        }
        else {
            var returnResult = result;

            deleteVersionEdits(data.reportVersionId, 20, function (result) {
                /* if error occurred during database communication then
                   it wil
                   l return error result otherwise it will continue for further process */
                if (!result.success) {
                    callback(result); /* return callback result */
                }
                else {
                    callback(returnResult);
                }
            });
        }
    })
}

/* create function to save last 20 edits according to report version */
function saveVersionEdits(data, reportPlugins, lastReportPlugin, callback) {

    /* create and execute query to save version edit */
    var query = "call spSaveVersionEdit(" + data.reportVersionId + ",'" + data.editType + "')";

     executeQuery(query, function (result) {
        /* if error occurred during database communication then
           it will return error result otherwise it will continue for further process */
        if (!result.success) {
            callback(result);
        }
        else {
            var versionEditId = result.message[0][0].ID;

            if (data.editType == 'single') {
                /* create and execute query to save content of version edit */
                query = "call spSaveVersionEditContent(" + lastReportPlugin.pluginTypeId + "," + lastReportPlugin.sort + ",'" + lastReportPlugin.parameterValue + "'," + versionEditId + "," + lastReportPlugin.id + ")";

                executeQuery(query, function (result) {
                    callback(result);  /* return callback result */
                })
            }
            else {
                var j = 0;
                for (var i = 0; i < reportPlugins.length; i++) {
                    /* create and execute query to save content of version edit */
                    query = "call spSaveVersionEditContent(" + reportPlugins[i].pluginTypeId + "," + reportPlugins[i].sort + ",'" + reportPlugins[i].parameterValue + "'," + versionEditId + "," + reportPlugins[i].id + ")";

                    executeQuery(query, function (result) {
                        if (j == reportPlugins.length - 1) {
                            callback(result); /* return callback result */
                        }
                        j++;
                    })
                }
            }
        }
    })
}


/* create method to save plugin chart data when save new report version in database */
function saveNewVersionPluginChartData(newReportVersionId, reportPlugins, callback) {

    /* get new report plugins according to new reportversionid */
    dbLogic.getReportPlugins(newReportVersionId, function (result) {

        var newReportPlugins = result.message[0];

        var j = 0;
        for (var i = 0; i < newReportPlugins.length; i++) {
            /* if plugin is line chart or heat map then save chart data in database from old report plugins */
            if (newReportPlugins[i].pluginTypeId == 1 || newReportPlugins[i].pluginTypeId == 4) {
                var data = reportPlugins.filter(function (item) {
                    return item.pluginTypeId == newReportPlugins[i].pluginTypeId;
                    // return item.pluginTypeId == newReportPlugins[i].pluginTypeId && item.sort == newReportPlugins[i].sort;
                });
                var pluginData = data[0];
                /* call method to save pluginchart data */
                dbLogic.savePluginChart(pluginData.chartData, newReportPlugins[i].id, function (result) {
                    if (j == newReportPlugins.length - 1) {
                        callback(result); /* if all data will be save then callback method will be called */
                    }
                    j++;
                })
            }
            else {
                j++;
            }
        }
    })
}

/* create method to delete version edit by reportversionid deom database */
function deleteVersionEdits(reportVersionId, value, callback) {
    /* create and execute query to get all version edits according to reportversionid */
    var query = "select * from versionedit where reportversionid=" + reportVersionId + " order by id desc";

    executeQuery(query, function (result) {
        /* if error occurred during database communication then
           it will return error result otherwise it will continue for further process */
        if (!result.success) {
            callback(result); /* return callback result */
        }
        else {
            var versionEdits = result.message;

            if (versionEdits.length > value) {
                var j = value;
                for (var i = value; i < versionEdits.length; i++) {
                    /* create and execute query to delete version edit */
                    query = "call spDeleteVersionEdit(" + versionEdits[i].id + ")";

                    executeQuery(query, function (result) {
                        /* if error occurred during database communication then
                           it will return error result otherwise it will continue for further process */
                        if (!result.success) {
                            callback(result); /* return callback result */
                        }
                        else {
                            if (j == versionEdits.length - 1) {
                                callback(result);
                            }
                            j++;
                        }
                    })
                }
            }
            else {
                callback(result);
            }
        }
    });
}

/* create function to execute all database queries */
function executeQuery(query, callback) {

    /* execute query to communicate with database */
    connection.query(query, function (err, recordset) {
        /* if error occured during database communication
           then it will log error message in console
           and return result with error message
           otherwise it will return result with success message */
        var obj = { success: false, message: err };
        if (err) {
            console.log(err);
        }
        else {
            obj.success = true;
            obj.message = recordset;
        }
        /* return callback result */
        callback(obj);
     })
}


var port = process.env.port || 3307;

/* run application in 8081 port */
var server = app.listen(8081, function () {

    var host = server.address().address
    var port = server.address().port

    console.log("Avinor api listening at http://%s:%s", host, port)

})


module.exports = app;

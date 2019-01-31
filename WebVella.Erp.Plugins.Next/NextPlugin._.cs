﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebVella.Erp.Api;
using WebVella.Erp.Api.Models;
using WebVella.Erp.Database;
using WebVella.Erp.Exceptions;
using WebVella.Erp.Plugins.Next.Model;

namespace WebVella.Erp.Plugins.Next
{
	public partial class NextPlugin : ErpPlugin
	{
		private const int WEBVELLA_NEXT_INIT_VERSION = 20190101;

		public void ProcessPatches()
		{
			using (SecurityContext.OpenSystemScope())
			{

				var entMan = new EntityManager();
				var relMan = new EntityRelationManager();
				var recMan = new RecordManager();
				var storeSystemSettings = DbContext.Current.SettingsRepository.Read();
				var systemSettings = new SystemSettings(storeSystemSettings);

				//Create transaction
				using (var connection = DbContext.Current.CreateConnection())
				{
					try
					{
						connection.BeginTransaction();

						//Here we need to initialize or update the environment based on the plugin requirements.
						//The default place for the plugin data is the "plugin_data" entity -> the "data" text field, which is used to store stringified JSON
						//containing the plugin settings or version

						//TODO: Develop a way to check for installed plugins
						#region << 1.Get the current ERP database version and checks for other plugin dependencies >>

						if (systemSettings.Version > 0)
						{
							//Do something if database version is not what you expect
						}

						//This plugin needs the webvella-sdk plugin to be installed, so we will check this here
						//var installedPlugins = new PluginService().Plugins;
						//var corePluginFound = false;
						//foreach (var plugin in installedPlugins)
						//{
						//	if (plugin.Name == "webvella-core")
						//	{
						//		corePluginFound = true;
						//		break;
						//	}
						//}

						//if (!corePluginFound)
						//	throw new Exception("'webvella-sdk' plugin is required for the 'webvella-sdk' to operate");

						#endregion

						#region << 2.Get the current plugin settings from the database >>

						var currentPluginSettings = new PluginSettings() { Version = WEBVELLA_NEXT_INIT_VERSION };
						string jsonData = GetPluginData();
						if (!string.IsNullOrWhiteSpace(jsonData))
							currentPluginSettings = JsonConvert.DeserializeObject<PluginSettings>(jsonData);

						#endregion

						#region << 3. Run methods based on the current installed version of the plugin >>

						
						//PATCH START
						{
							var patchVersion = 20190117;
							if (currentPluginSettings.Version < patchVersion)
							{
								try
								{
									currentPluginSettings.Version = patchVersion;
									Patch20190117(entMan, relMan, recMan);
								}
								catch (ValidationException ex)
								{
									var exception = ex;
									throw ex;
								}
								catch (Exception ex)
								{
									var exception = ex;
									throw ex;
								}
							}
						}

						//PATCH 20190122
						{
							var patchVersion = 20190122;
							if (currentPluginSettings.Version < patchVersion)
							{
								try
								{
									currentPluginSettings.Version = patchVersion;
									Patch20190122(entMan, relMan, recMan);
								}
								catch (ValidationException ex)
								{
									var exception = ex;
									throw ex;
								}
								catch (Exception ex)
								{
									var exception = ex;
									throw ex;
								}
							}
						}

						//PATCH 20190127
						{
							var patchVersion = 20190127;
							if (currentPluginSettings.Version < patchVersion)
							{
								try
								{
									currentPluginSettings.Version = patchVersion;
									Patch20190127(entMan, relMan, recMan);
								}
								catch (ValidationException ex)
								{
									var exception = ex;
									throw ex;
								}
								catch (Exception ex)
								{
									var exception = ex;
									throw ex;
								}
							}
						}

						//PATCH 20190128
						{
							var patchVersion = 20190128;
							if (currentPluginSettings.Version < patchVersion)
							{
								try
								{
									currentPluginSettings.Version = patchVersion;
									Patch20190128(entMan, relMan, recMan);
								}
								catch (ValidationException ex)
								{
									var exception = ex;
									throw ex;
								}
								catch (Exception ex)
								{
									var exception = ex;
									throw ex;
								}
							}
						}


						//PATCH 20190129
						{
							var patchVersion = 20190129;
							if (currentPluginSettings.Version < patchVersion)
							{
								try
								{
									currentPluginSettings.Version = patchVersion;
									Patch20190129(entMan, relMan, recMan);
								}
								catch (ValidationException ex)
								{
									var exception = ex;
									throw ex;
								}
								catch (Exception ex)
								{
									var exception = ex;
									throw ex;
								}
							}
						}
						#endregion


						SavePluginData(JsonConvert.SerializeObject(currentPluginSettings));

						connection.CommitTransaction();
						//connection.RollbackTransaction();
					}
					catch (ValidationException ex)
					{
						connection.RollbackTransaction();
						throw ex;
					}
					catch (Exception ex)
					{
						connection.RollbackTransaction();
						throw ex;
					}
				}
			}
		}
	}
}
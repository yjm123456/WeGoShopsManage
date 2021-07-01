using BaseClass;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.SqlDal.Store;
using System;
using System.Collections.Generic;
using System.Data;

namespace Hidistro.ControlPanel.Store
{
	public static class GalleryHelper
	{
        public static bool AddPhote(int categoryId, string photoName, string photoPath, int fileSize,string TenantID)
		{
			return (new PhotoGalleryDao()).AddPhote(categoryId, photoName, photoPath, fileSize,TenantID);
		}

		public static  bool AddPhotoCategory(string name,string TenantID)
		{
			return (new PhotoGalleryDao()).AddPhotoCategory(name, TenantID);
		}

		public static int AddPhotoCategory2(string name,string TenantID)
		{
			return (new PhotoGalleryDao()).AddPhotoCategory2(name,TenantID);
		}

		public static bool DeletePhoto(int photoId)
		{
			string photoPath = GalleryHelper.GetPhotoPath(photoId);
			bool flag = (new PhotoGalleryDao()).DeletePhoto(photoId);
			if (flag)
			{
				StoreHelper.DeleteImage(photoPath);
			}
			return flag;
		}

		public static bool DeletePhotoCategory(int categoryId)
		{
			return (new PhotoGalleryDao()).DeletePhotoCategory(categoryId);
		}

		public static int GetDefaultPhotoCount(string TenantID)
		{
			return (new PhotoGalleryDao()).GetDefaultPhotoCount(TenantID);
		}

		public static DataTable GetPhotoCategories(int type,string TenantID)
		{
			return (new PhotoGalleryDao()).GetPhotoCategories(type,TenantID);
		}

		public static int GetPhotoCount()
		{
			return (new PhotoGalleryDao()).GetPhotoCount();
		}

		public static DbQueryResult GetPhotoList(string keyword, int? categoryId, int pageIndex, PhotoListOrder order, int type, int pagesize = 20)
		{
			DbQueryResult photoList = GalleryHelper.GetPhotoList(keyword, categoryId, pageIndex, pagesize, order, type);
			return photoList;
		}

		public static DbQueryResult GetPhotoList(string keyword, int? categoryId, int pageIndex, int pageSize, PhotoListOrder order, int type)
		{
			Pagination pagination = new Pagination()
			{
				PageSize = pageSize,
				PageIndex = pageIndex,
				IsCount = true
			};
			switch (order)
			{
				case PhotoListOrder.UploadTimeDesc:
				{
					pagination.SortBy = "UploadTime";
					pagination.SortOrder = SortAction.Desc;
					break;
				}
				case PhotoListOrder.UploadTimeAsc:
				{
					pagination.SortBy = "UploadTime";
					pagination.SortOrder = SortAction.Asc;
					break;
				}
				case PhotoListOrder.NameAsc:
				{
					pagination.SortBy = "PhotoName";
					pagination.SortOrder = SortAction.Asc;
					break;
				}
				case PhotoListOrder.NameDesc:
				{
					pagination.SortBy = "PhotoName";
					pagination.SortOrder = SortAction.Desc;
					break;
				}
				case PhotoListOrder.UpdateTimeDesc:
				{
					pagination.SortBy = "LastUpdateTime";
					pagination.SortOrder = SortAction.Desc;
					break;
				}
				case PhotoListOrder.UpdateTimeAsc:
				{
					pagination.SortBy = "LastUpdateTime";
					pagination.SortOrder = SortAction.Asc;
					break;
				}
				case PhotoListOrder.SizeDesc:
				{
					pagination.SortBy = "FileSize";
					pagination.SortOrder = SortAction.Desc;
					break;
				}
				case PhotoListOrder.SizeAsc:
				{
					pagination.SortBy = "FileSize";
					pagination.SortOrder = SortAction.Asc;
					break;
				}
			}
            string TenantID = ManagerHelper.GetCurrentTenantID();
            return (new PhotoGalleryDao()).GetPhotoList(keyword, categoryId, pagination, type,TenantID);
		}

		public static string GetPhotoPath(int photoId)
		{
			return (new PhotoGalleryDao()).GetPhotoPath(photoId);
		}

		public static int MovePhotoType(List<int> pList, int pTypeId)
		{
			return (new PhotoGalleryDao()).MovePhotoType(pList, pTypeId);
		}

		public static void RenamePhoto(int photoId, string newName)
		{
			(new PhotoGalleryDao()).RenamePhoto(photoId, newName);
		}

		public static void ReplacePhoto(int photoId, int fileSize)
		{
			(new PhotoGalleryDao()).ReplacePhoto(photoId, fileSize);
		}

		public static void SwapSequence(int categoryId1, int categoryId2)
		{
			(new PhotoGalleryDao()).SwapSequence(categoryId1, categoryId2);
		}

		public static int UpdatePhotoCategories(Dictionary<int, string> photoCategorys)
		{
			return (new PhotoGalleryDao()).UpdatePhotoCategories(photoCategorys);
		}
	}
}
﻿//***************************************************
//
//  Author: Ben Hopkins
//  Copyright (C) 2016 kode80 LLC, 
//  all rights reserved
// 
//  Free to use for non-commercial purposes, 
//  see full license in project root:
//  PixelRenderNonCommercialLicense.html
//  
//  Commercial licenses available for purchase from:
//  http://kode80.com/
//
//***************************************************

using UnityEngine;

namespace kode80.PixelRender
{
	[ExecuteInEditMode]
	public class FullScreenQuad : MonoBehaviour 
	{
		public Material material;
		public bool renderWhenPlaying;
		private Mesh _mesh;


		public float zScale = 1.0f;

		void Awake()
		{
			_mesh = new Mesh();
			_mesh.hideFlags = HideFlags.HideAndDontSave;
		}

		// Use this for initialization
		void Start () {
		}
		
		void OnRenderObject()
		{
			Camera camera = Camera.current;

			if( (Application.isPlaying && renderWhenPlaying == false) || 
				material == null || 
				(camera.cullingMask & (1 << gameObject.layer)) == 0)
			{
				return;
			}

			int w = camera.pixelWidth;
			int h = camera.pixelHeight;
			float z = camera.farClipPlane * zScale;

			//Debug.Log( "render: " + w + " , " + h + " , " + z);

			Vector3 v0 = camera.ScreenToWorldPoint( new Vector3( 0.0f, 0.0f, z));
			Vector3 v1 = camera.ScreenToWorldPoint( new Vector3( w, 0.0f, z));
			Vector3 v2 = camera.ScreenToWorldPoint( new Vector3( w, h, z));
			Vector3 v3 = camera.ScreenToWorldPoint( new Vector3( 0.0f, h, z));

//			Vector3 v0 = new Vector3( 0.0f, 0.0f, z);
//			Vector3 v1 = new Vector3( w, 0.0f, z);
//			Vector3 v2 = new Vector3( w, h, z);
//			Vector3 v3 = new Vector3( 0.0f, h, z);

			Vector2 uv0 = new Vector2( 0.0f, 0.0f);
			Vector2 uv1 = new Vector2( 1.0f, 0.0f);
			Vector2 uv2 = new Vector2( 1.0f, 1.0f);
			Vector2 uv3 = new Vector2( 0.0f, 1.0f);
			
			_mesh.vertices = new Vector3[] { v0, v1, v2, v3 };
			_mesh.uv = new Vector2[] { uv0, uv1, uv2, uv3 };
			_mesh.triangles = new int[] { 0, 1, 2, 
										 2, 3, 0};
			_mesh.RecalculateBounds();
			//_mesh.RecalculateNormals();
			//_mesh.UploadMeshData( false);

			material.SetPass( 0);
			Graphics.DrawMeshNow( _mesh, Vector3.zero, Quaternion.identity, 0);
		}
	}
}
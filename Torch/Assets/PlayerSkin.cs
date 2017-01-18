using UnityEngine;
using System.Collections;
using System;

public class PlayerSkin : MonoBehaviour {

	public SkinnedMeshRenderer BodyRightHalf;
	public SkinnedMeshRenderer BodyLeftHalf;
	public MeshRenderer Hat;
	public MeshRenderer Staff;

	[Serializable]
	public struct OriginalMaterials{
		public Material clothing;
		public Material skin;
		public Material staff;
		public Material cape;
		public Material hat;
	}

	public Material randomMaterial;

	public OriginalMaterials originalMaterials;

	void Update(){
	/*	if (Input.GetKeyDown (KeyCode.Alpha7)) {
			ResetMaterials ();
		}
		*/
	}

	public void SetHatSkin(Material material){
		Hat.material = material;
	}

	/*
	public void SetCapeSkin(Material material){		
		Hat.material = material;

		Material materials = BodyRightHalf.materials;
		materials [0] = material;
		BodyRightHalf.materials = materials;

		materials = BodyLeftHalf.materials;
		materials[0] = material;
		materials[1] = material;
		materials[2] = material;
		BodyLeftHalf.materials = materials;
	}
	*/

	public void SetCapeSkin(Material material){
		Material[] materials = BodyRightHalf.materials;
		materials [0] = material;
		BodyRightHalf.materials = materials;
	}

	/*
	public void SetSkinSkin(Material material){
		BodyRightHalf.materials [1] = material;
		BodyRightHalf.materials [2] = material;
		BodyLeftHalf.materials [0] = material;
		BodyLeftHalf.materials [2] = material;
	}

	public void SetStaffSkin(Material material){
		Hat.material = material;
	}

	public void SetClothingSkin(Material material){
		BodyRightHalf.materials [3] = material;
		BodyLeftHalf.materials [1] = material;
	}
*/

	public void ResetMaterials(){
		Hat.material = originalMaterials.hat;

		Material[] materials = BodyRightHalf.materials;
		materials [0] = originalMaterials.cape;
		materials [1] = originalMaterials.skin;
		materials [2] = originalMaterials.skin;
		materials [3] = originalMaterials.clothing;
		BodyRightHalf.materials = materials;

		materials = BodyLeftHalf.materials;
		materials [0] = originalMaterials.skin;
		materials [1] = originalMaterials.clothing;
		materials [2] = originalMaterials.skin;
		BodyLeftHalf.materials = materials;

		Staff.material = originalMaterials.hat;

	}

}

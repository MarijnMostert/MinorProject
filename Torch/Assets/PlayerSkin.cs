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

	public OriginalMaterials originalMaterials;

	void Start(){
		originalMaterials.clothing = BodyRightHalf.materials [3];
		originalMaterials.skin = BodyRightHalf.materials [1];
		originalMaterials.staff = Staff.material;
		originalMaterials.cape = BodyRightHalf.materials [0];
		originalMaterials.hat = Hat.material;
	}

	public void SetHatSkin(Material material){
		Hat.material = material;
	}

	public void SetCapeSkin(Material material){
		BodyRightHalf.materials [0] = material;
		Hat.material = material;
		BodyLeftHalf.materials[0] = material;
		BodyLeftHalf.materials[1] = material;
		BodyLeftHalf.materials[2] = material;
	}

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

	public void ResetMaterials(){
		Hat.material = originalMaterials.hat;
		BodyRightHalf.materials [0] = originalMaterials.cape;
		BodyRightHalf.materials [1] = originalMaterials.skin;
		BodyRightHalf.materials [2] = originalMaterials.skin;
		BodyRightHalf.materials [3] = originalMaterials.clothing;
		BodyLeftHalf.materials [0] = originalMaterials.skin;
		BodyLeftHalf.materials [1] = originalMaterials.clothing;
		BodyLeftHalf.materials [2] = originalMaterials.skin;
		Staff.material = originalMaterials.hat;

	}

}

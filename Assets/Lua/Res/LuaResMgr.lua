---@class resMgr
local resMgr = CS.ResMgr.Instance


print("lua资源加载启动")
print(resMgr)
local res = resMgr:Test("Prefab/Test.prefab")

local obj = GameObject.Instantiate(res)
obj.name = 'wxy_test'
local trans = obj:GetComponent(typeof(CS.UnityEngine.Transform))
print(trans)
trans.position = CS.UnityEngine.Vector3(1,1,1)
--obj.transform.position = Vector3.zero

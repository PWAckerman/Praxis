using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceManager {

	public int ResourceA = 0;
	public int ResourceB = 0;
	public int ResourceC = 0;
	public int ResourceD = 0;
	public int RefinedA = 0;
	public int RefinedB = 0;
	public int RefinedC = 0;
	public int RefinedD = 0;
	public int MinerA = 0;
	public int MinerB = 0;
	public int MinerC = 0;
	public int MinerD = 0;
	public int MinerCost = 1000;
	public int Gold = 0;
	public int WIDGET_INVENTORY = 0;

	//Power Generation Factors
	public int GENERATORS = 0;
	public int GeneratorCost = 1000;
	public float POWER_EFFICIENCY = .5f;

	//Procurement Factors
	public float MINER_EFFICIENCY = 0.5f;
	public int PROCURE_BASE_YIELD = 2;

	//Refinement Factors
	public float REFINERY_EFFICIENCY = 0.5f;
	public int REFINERIES = 0;
	public int REFINE_BASE_YIELD = 20;
	public int RefineryCost = 1000;

	//Production Factors
	public int FACTORIES = 0;
	public int PRODUCTION_BASE_YIELD = 100;
	public float PRODUCTION_EFFICIENCY = 0.5f;
	public int FactoryCost = 1000;

	//Marketing Factors
	public int MARKETING_LEVEL = 1;
	public int MarketingCost = 1000;

	//Crypto Currency Mining
	public int CRYPTO = 0;
	public int CRYPTO_NODES = 0;
	public float CRYPTO_DIFFICULTY = .2f;
	public int NodeCost = 1000;

	//Sales & Distribution Factors
	public int DEMAND = 100;
	public int DISTRIBUTION_FORCE = 1;
	public int BASE_PRICE = 100;
	public int TruckCost = 1000;


	//Workforce Stats
	public int WORKFORCE = 0;
	public float UNION_INFLUENCE = 0f;
	public int BASE_SALARY = 10000;
	private static ResourceManager rm = null;
	private long iteration = 0;

	// Use this for initialization
	private ResourceManager(){
		
	}

	public static ResourceManager getResourceManager(){
		if (rm != null) {
			return rm;
		} else {
			rm = new ResourceManager();
			return rm;
		}
	}

	public void Run() {
		//Is the system generating enough power to support operations?
		iteration++;
		float powerNeeds = this.CalculatePower();
		float powerGenerated = (this.GENERATORS * this.POWER_EFFICIENCY * 1000);

		if(powerNeeds < powerGenerated){
			this.CalculateRPForAll();
			this.CalculateRefinementForAll();
			this.CalculateProduction();
			this.CalculateCrypto();
		}
		//We sell existing inventory, even if we're not producing
		this.CalculateEntityDemand();
		//We pay our employees, even if we're not producing
		this.CalculatePayroll();
	}

	public void increaseResource(LootTypes tp){
		switch(tp){
			case LootTypes.A:
				ResourceA++;
				break;
			case LootTypes.B:
				ResourceB++;
				break;
			case LootTypes.C:
				ResourceC++;
				break;
			case LootTypes.D:
				ResourceD++;
				break;
			default:
				break;
		}
		return;
	}

	public void increaseGold(){
		Gold = Gold + 50;
	}

	public int GetNeededResources(){
		float manufacturingEffectiveness = 1f/this.PRODUCTION_EFFICIENCY;
		int neededResources = (int)Mathf.Ceil((manufacturingEffectiveness * 10));
		return neededResources;
	}

	public int GetNeededManResources(){
		float refineryEffectiveness = 1/this.REFINERY_EFFICIENCY;
		int neededResources = (int)Mathf.Ceil((refineryEffectiveness * 10) * 1);
		return neededResources;
	}

	public int GetCostOfGenerator(){
		return (int)Mathf.Ceil(GeneratorCost * Mathf.Pow((float)GENERATORS + 1f, 1.1f));
	}

	public int GetMinerCost(string miner){
		switch (miner) {
		case "A":
			return (int)Mathf.Ceil (MinerCost * Mathf.Pow((float)MinerA + 1, 1.1f));
		case "B":
			return (int)Mathf.Ceil (MinerCost * Mathf.Pow((float)MinerB + 1, 1.1f));
		case "C":
			return (int)Mathf.Ceil (MinerCost * Mathf.Pow((float)MinerC + 1, 1.1f));
		case "D":
			return (int)Mathf.Ceil (MinerCost * Mathf.Pow((float)MinerD + 1, 1.1f));
		default:
			return 0;
		}
	}

	void CalculateEntityDemand(){
		//How much we can sell, based on our marketing effectiveness and baseline demand
		if (iteration % 100 == 0) {
			int internalDemand = this.DEMAND * this.MARKETING_LEVEL;
			int subtraction = 0;
			//How much of what we distribute will actually make it to the sales floor
			//Distribution Force: How many trucks we have
			float distributionEffectiveness = this.DISTRIBUTION_FORCE / 20f;
			if (WIDGET_INVENTORY > internalDemand) {
				subtraction = internalDemand;
			} else if (WIDGET_INVENTORY < internalDemand) {
				subtraction = WIDGET_INVENTORY;
			} 
			if (subtraction > 0) {
				this.WIDGET_INVENTORY = this.WIDGET_INVENTORY - subtraction;
				Debug.Log ("MY SHIT");
				Debug.Log (subtraction);
				Debug.Log (distributionEffectiveness);
				this.Gold = (int)Mathf.Ceil (this.Gold + ((subtraction * distributionEffectiveness) * this.BASE_PRICE));
			}
		}
	}
		
	void CalculateRPForAll(){
		if (iteration % 10 == 0) {
			this.ResourceA = this.ResourceA + (this.MinerA * (int)Mathf.Ceil (this.MINER_EFFICIENCY * this.PROCURE_BASE_YIELD));
			this.ResourceB = this.ResourceB + (this.MinerB * (int)Mathf.Ceil (this.MINER_EFFICIENCY * this.PROCURE_BASE_YIELD));
			this.ResourceC = this.ResourceC + (this.MinerC * (int)Mathf.Ceil (this.MINER_EFFICIENCY * this.PROCURE_BASE_YIELD));
			this.ResourceD = this.ResourceD + (this.MinerD * (int)Mathf.Ceil (this.MINER_EFFICIENCY * this.PROCURE_BASE_YIELD));
		}
	}

	public void PurchaseMiner(string miner){
		switch (miner) {
			case "A":
			if (this.Gold > (int)Mathf.Ceil(MinerCost * Mathf.Pow((float)MinerA + 1f, 1.1f))) {
				
				this.Gold -= (int)Mathf.Ceil(MinerCost * Mathf.Pow((float)MinerA + 1f, 1.1f));
				this.MinerA++;
				}
			break;
			case "B":
			if (this.Gold > (int)Mathf.Ceil(MinerCost * Mathf.Pow((float)MinerB + 1f, 1.1f))) {
				
				this.Gold -= (int)Mathf.Ceil(MinerCost * Mathf.Pow((float)MinerB + 1f, 1.1f));
				this.MinerB++;
				}
			break;
			case "C":
			if (this.Gold > (int)Mathf.Ceil(MinerCost * Mathf.Pow((float)MinerC + 1f, 1.1f))) {
				
				this.Gold -= (int)Mathf.Ceil(MinerCost * Mathf.Pow((float)MinerC + 1f, 1.1f));
				this.MinerC++;
			}
			break;
			case "D":
			if (this.Gold > (int)Mathf.Ceil(MinerCost * Mathf.Pow((float)MinerD + 1f, 1.1f))) {
				
				this.Gold -= (int)Mathf.Ceil(MinerCost * Mathf.Pow((float)MinerD + 1f, 1.1f));
				this.MinerD++;
			}
			break;
			default:
				return;
		}

	}

	public void PurchaseGenerator(){
		if (this.Gold > (int)Mathf.Ceil(GeneratorCost * Mathf.Pow((float)GENERATORS + 1f, 1.1f))) {
			this.Gold -= (int)Mathf.Ceil(GeneratorCost * Mathf.Pow((float)GENERATORS + 1f, 1.1f));
			this.GENERATORS++;
		}
	}

	public void PurchaseCrypto(){
		if (this.Gold > (int)Mathf.Ceil(NodeCost * Mathf.Pow((float)CRYPTO_NODES + 1f, 1.1f))) {
			this.Gold -= (int)Mathf.Ceil(NodeCost * Mathf.Pow((float)CRYPTO_NODES + 1f, 1.1f));
			this.CRYPTO_NODES++;
		}
	}

	public int GetNodeCost(){
		return (int)Mathf.Ceil(NodeCost * Mathf.Pow((float)CRYPTO_NODES + 1f, 1.1f));
	}
		
	void CalculateProduction(){
		float manufacturingEffectiveness = 1f/this.PRODUCTION_EFFICIENCY;
		Debug.Log (manufacturingEffectiveness);
		float neededResources = (manufacturingEffectiveness * 10) * this.FACTORIES;
		List<int> available = new List<int> (){ RefinedA, RefinedB, RefinedC, RefinedD };
		if(available.TrueForAll((int val)=>{
			return val >= neededResources;
		})){

			RefinedA = (int)Mathf.Ceil(RefinedA - neededResources);
			RefinedB = (int)Mathf.Ceil(RefinedB - neededResources);
			RefinedC = (int)Mathf.Ceil(RefinedC - neededResources);
			RefinedD = (int)Mathf.Ceil(RefinedD - neededResources);
			this.WIDGET_INVENTORY = this.WIDGET_INVENTORY + Mathf.RoundToInt((this.PRODUCTION_BASE_YIELD + this.WORKFORCE) * (this.FACTORIES * this.PRODUCTION_EFFICIENCY));
		}
	}

	public void PurchaseFactory(){
		if (this.Gold > (int)Mathf.Ceil(FactoryCost * Mathf.Pow((float)FACTORIES + 1f, 1.1f))) {
			this.Gold -= (int)Mathf.Ceil(FactoryCost * Mathf.Pow((float)FACTORIES + 1f, 1.1f));
			this.FACTORIES++;
		}
	}

	public int GetFactoryCost(){
		return (int)Mathf.Ceil(FactoryCost * Mathf.Pow((float)FACTORIES + 1f, 1.1f));
	}

	public void IncreaseMarketingLevel(){
		if (this.Gold > (int)Mathf.Ceil(MarketingCost * Mathf.Pow((float)MARKETING_LEVEL + 1f, 1.1f))) {
			this.Gold -= (int)Mathf.Ceil(MarketingCost * Mathf.Pow((float)MARKETING_LEVEL + 1f, 1.1f));
			this.MARKETING_LEVEL++;
		}
	}

	public void PurchaseTruck(){
		if (this.Gold > (int)Mathf.Ceil(TruckCost * Mathf.Pow((float)DISTRIBUTION_FORCE + 1f, 1.1f))) {
			this.Gold -= (int)Mathf.Ceil(TruckCost * Mathf.Pow((float)DISTRIBUTION_FORCE + 1f, 1.1f));
			this.DISTRIBUTION_FORCE++;
		}
	}

	public void PurchaseRefinery(){
		if (this.Gold > (int)Mathf.Ceil(RefineryCost * Mathf.Pow((float)REFINERIES + 1f, 1.1f))) {
			this.Gold -= (int)Mathf.Ceil(RefineryCost * Mathf.Pow((float)REFINERIES + 1f, 1.1f));
			this.REFINERIES++;
		}
	}


	public int GetMarketingCost(){
		return (int)Mathf.Ceil(MarketingCost * Mathf.Pow((float)MARKETING_LEVEL + 1f, 1.1f));
	}

	public int GetTruckCost(){
		return (int)Mathf.Ceil(TruckCost * Mathf.Pow((float)DISTRIBUTION_FORCE + 1f, 1.1f));
	}

	public int GetRefineryCost(){
		return (int)Mathf.Ceil(RefineryCost * Mathf.Pow((float)REFINERIES + 1f, 1.1f));
	}


	void CalculateCrypto(){
		if (iteration % 100 == 0) {
			this.CRYPTO = (int)Mathf.Ceil (this.CRYPTO + (this.CRYPTO_NODES * this.CRYPTO_DIFFICULTY));
		}
	}
		

	void CalculateRefinementForAll(){
		float refineryEffectiveness = 1/this.REFINERY_EFFICIENCY;
		float neededResources = (refineryEffectiveness * 10) * this.REFINERIES;
		if(this.ResourceA > neededResources){
			this.ResourceA = (int)Mathf.Ceil(this.ResourceA - neededResources);
			this.RefinedA = Mathf.RoundToInt(this.RefinedA + (this.REFINE_BASE_YIELD * (this.REFINERIES * this.REFINERY_EFFICIENCY)));
		}
		if(this.ResourceB > neededResources){
			this.ResourceB = (int)Mathf.Ceil(this.ResourceB - neededResources);
			this.RefinedB = Mathf.RoundToInt(this.RefinedB + (this.REFINE_BASE_YIELD * (this.REFINERIES * this.REFINERY_EFFICIENCY)));
		}
		if(this.ResourceC > neededResources){
			this.ResourceC = (int)Mathf.Ceil(this.ResourceC - neededResources);
			this.RefinedC = Mathf.RoundToInt(this.RefinedC + (this.REFINE_BASE_YIELD * (this.REFINERIES * this.REFINERY_EFFICIENCY)));
		}
		if(this.ResourceD > neededResources){
			this.ResourceD = (int)Mathf.Ceil(this.ResourceD - neededResources);
			this.RefinedD = Mathf.RoundToInt(this.RefinedD + (this.REFINE_BASE_YIELD * (this.REFINERIES * this.REFINERY_EFFICIENCY)));
		}
	}

	void CalculatePayroll(){
		this.Gold = (int)Mathf.Ceil(this.Gold - (this.WORKFORCE * (this.UNION_INFLUENCE * this.BASE_SALARY)));
	}

	public float CalculatePower(){
		float POWER_COEFFICIENT = 1/POWER_EFFICIENCY;
		int totalMiners = 0;		
		totalMiners = MinerA + MinerB + MinerC + MinerD;
		float miningPowerConsumption = totalMiners * 10 * POWER_COEFFICIENT;
		float refineryPowerConsumption = this.REFINERIES * 100 * POWER_COEFFICIENT;
		float factoryPowerConsumption = this.FACTORIES * 100 * POWER_COEFFICIENT;
		float cryptoPowerConsumption = this.CRYPTO_NODES * 20 * POWER_COEFFICIENT;
		return miningPowerConsumption + refineryPowerConsumption + factoryPowerConsumption + cryptoPowerConsumption;
	}

	public void ManuallyRefine(){
		float refineryEffectiveness = 1/this.REFINERY_EFFICIENCY;
		float neededResources = (refineryEffectiveness * 10) * 1;
		if(this.ResourceA > neededResources){
			this.ResourceA = (int)Mathf.Ceil(this.ResourceA - neededResources);
			this.RefinedA = Mathf.RoundToInt(this.RefinedA + (this.REFINE_BASE_YIELD * (1 * this.REFINERY_EFFICIENCY)));
		}
		if(this.ResourceB > neededResources){
			this.ResourceB = (int)Mathf.Ceil(this.ResourceB - neededResources);
			this.RefinedB = Mathf.RoundToInt(this.RefinedB + (this.REFINE_BASE_YIELD * (1 * this.REFINERY_EFFICIENCY)));
		}
		if(this.ResourceC > neededResources){
			this.ResourceC = (int)Mathf.Ceil(this.ResourceC - neededResources);
			this.RefinedC = Mathf.RoundToInt(this.RefinedC + (this.REFINE_BASE_YIELD * (1 * this.REFINERY_EFFICIENCY)));
		}
		if(this.ResourceD > neededResources){
			this.ResourceD = (int)Mathf.Ceil(this.ResourceD - neededResources);
			this.RefinedD = Mathf.RoundToInt(this.RefinedD + (this.REFINE_BASE_YIELD * (1 * this.REFINERY_EFFICIENCY)));
		}
	}

	public void ManuallyManufacture(){
		float manufacturingEffectiveness = 1f/this.PRODUCTION_EFFICIENCY;
		Debug.Log (manufacturingEffectiveness);
		float neededResources = (manufacturingEffectiveness * 10) * 1;
		Debug.Log ("Needed Resource");
		Debug.Log (neededResources);
		List<int> available = new List<int> (){ RefinedA, RefinedB, RefinedC, RefinedD };
		if(available.TrueForAll((int val)=>{
			return val >= neededResources;
		})){
			
			RefinedA = (int)Mathf.Ceil(RefinedA - neededResources);
			RefinedB = (int)Mathf.Ceil(RefinedB - neededResources);
			RefinedC = (int)Mathf.Ceil(RefinedC - neededResources);
			RefinedD = (int)Mathf.Ceil(RefinedD - neededResources);
			this.WIDGET_INVENTORY = this.WIDGET_INVENTORY + Mathf.RoundToInt((this.PRODUCTION_BASE_YIELD + this.WORKFORCE) * (1 * this.PRODUCTION_EFFICIENCY));
		}
	}
}

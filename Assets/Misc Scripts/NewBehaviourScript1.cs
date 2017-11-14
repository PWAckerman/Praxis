using UnityEngine;
using UnityEngine.UI;
 
using System;
using System.Timers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Simulator : MonoBehaviour {

	// Use this for initialization
	Dictionary<string, int> RESOURCE = new Dictionary<string, int>();

	//Starting Refined Amounts
	Dictionary<string, int> REFINED = new Dictionary<string, int>();
		//Starting Mining Equipment
	Dictionary<string, int> MINERS = new Dictionary<string, int>();

	//Starting Funds & Inventory
	int FUNDS = 1000000000;
	int WIDGET_INVENTORY = 10000000;

	//Power Generation Factors
	int GENERATORS = 25;
	double POWER_EFFICIENCY = .5;

	//Procurement Factors
	int MINER_EFFICIENCY = 1;
	int PROCURE_BASE_YIELD = 20;

	//Refinement Factors
	int REFINERY_EFFICIENCY = 2;
	int REFINERIES = 4;
	int REFINE_BASE_YIELD = 20;

	//Production Factors
	int FACTORIES = 5;
	int PRODUCTION_BASE_YIELD = 4000;
	int PRODUCTION_EFFICIENCY = 3;

	//Marketing Factors
	int MARKETING_LEVEL = 1000;

	//Crypto Currency Mining
	int CRYPTO = 100000;
	int CRYPTO_NODES = 100;
	double CRYPTO_DIFFICULTY = .2;

	//Sales & Distribution Factors
	int DEMAND = 100;
	int DISTRIBUTION_FORCE = 20;
	int BASE_PRICE = 100;

	//Workforce Stats
	int WORKFORCE = 4000;
	double UNION_INFLUENCE = .2;
	int BASE_SALARY = 10000;

	Text score;

	public Simulator(int ver){
		RESOURCE.Add("A", 100000);
		RESOURCE.Add("B", 100000);
		RESOURCE.Add("C", 100000);
		RESOURCE.Add("D", 100000);
		REFINED.Add("A", 100000);
		REFINED.Add("B", 100000);
		REFINED.Add("C", 100000);
		REFINED.Add("D", 100000);
		MINERS.Add("A", 50);
		MINERS.Add("B", 60);
		MINERS.Add("C", 40);
		MINERS.Add("D", 50);
		this.score = GameObject.FindWithTag("funds").GetComponent<Text>();
	}
	
	public void Start () {
		Timer timer = new Timer(1000);
      	timer.Elapsed += OnTimedEvent;
      	timer.Start();
	}

	public void Update () {
		this.score.text = "S" + this.FUNDS.ToString();
	}

	private void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
    {
        this.Run();
    }

	public int GetFunds(){
		return this.FUNDS;
	}

	void Run() {
		//Is the system generating enough power to support operations?
		double powerNeeds = this.CalculatePower();
		double powerGenerated = (this.GENERATORS * this.POWER_EFFICIENCY * 1000);
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
		this.PrintStatus(powerNeeds, powerGenerated);
	}

	void PrintStatus(double need, double gen){
		// Debug.Log("Inventory: ");
		// Debug.Log(this.WIDGET_INVENTORY);
		// Debug.Log("Resources: ");
		// Debug.Log(this.RESOURCE);
		// Debug.Log("Refined: ");
		// Debug.Log(this.REFINED);
		// Debug.Log("Funds: ");
		// Debug.Log(this.FUNDS);
		// Debug.Log("Crypto: ");
		// Debug.Log(this.CRYPTO);
		// Debug.Log("Power: ");
		// Debug.Log(need/gen);
	}

	void CalculateEntityDemand(){
		//How much we can sell, based on our marketing effectiveness and baseline demand
		int internalDemand = this.DEMAND * this.MARKETING_LEVEL;
		//How much of what we distribute will actually make it to the sales floor
		//Distribution Force: How many trucks we have
		double distributionEffectiveness = this.DISTRIBUTION_FORCE/20;
		if(internalDemand < this.WIDGET_INVENTORY){
			this.WIDGET_INVENTORY = this.WIDGET_INVENTORY - internalDemand;
			this.FUNDS = (int)Math.Ceiling(this.FUNDS + ((internalDemand * distributionEffectiveness) * this.BASE_PRICE));
		}
	}

	void CalculateResourceProcurement(string rType){
		// additional resources are dependent on how many miners there are, how efficient they are, and what the base yield is for a particular resource
		this.RESOURCE[rType] = this.RESOURCE[rType] + (this.MINERS[rType] * (this.MINER_EFFICIENCY * this.PROCURE_BASE_YIELD));
	}

	void CalculateRPForAll(){
		List<string> kees = new List<string>(this.RESOURCE.Keys);
		foreach (string key in kees){
			this.CalculateResourceProcurement(key);
		}
	}


	void CalculateProduction(){
		double manufacturingEffectiveness = 1/this.PRODUCTION_EFFICIENCY;
		double neededResources = (manufacturingEffectiveness * 100) * this.FACTORIES;
		List<int> available = this.REFINED.Values.ToList();
		if(available.TrueForAll((int val)=>{
			return val > neededResources;
		})){
			List<string> kees = new List<string>(this.RESOURCE.Keys);
			foreach (string key in kees){
				this.REFINED[key] = (int)Math.Ceiling(this.REFINED[key] - neededResources);
			}
			this.WIDGET_INVENTORY = this.WIDGET_INVENTORY + ((this.PRODUCTION_BASE_YIELD + this.WORKFORCE) * (this.FACTORIES * this.PRODUCTION_EFFICIENCY));
		}
	}

	void CalculateCrypto(){
		this.CRYPTO = (int)Math.Ceiling(this.CRYPTO + (this.CRYPTO_NODES * this.CRYPTO_DIFFICULTY));
	}

	void CalculateResourceRefinement(string rType){
		double refineryEffectiveness = 1/this.REFINERY_EFFICIENCY;
		double neededResources = (refineryEffectiveness * 100) * this.REFINERIES;
		if(this.RESOURCE[rType] > neededResources){
			this.RESOURCE[rType] = (int)Math.Ceiling(this.RESOURCE[rType] - neededResources);
			this.REFINED[rType] = this.REFINED[rType] + (this.REFINE_BASE_YIELD * (this.REFINERIES * this.REFINERY_EFFICIENCY));
		}
	}

	void CalculateRefinementForAll(){
		List<string> kees = new List<string>(this.RESOURCE.Keys);
		foreach (string key in kees){
			this.CalculateResourceRefinement(key);
		}
	}

	void CalculatePayroll(){
		this.FUNDS = (int)Math.Ceiling(this.FUNDS - (this.WORKFORCE * (this.UNION_INFLUENCE * this.BASE_SALARY)));
	}

	double CalculatePower(){
		double POWER_COEFFICIENT = 1/POWER_EFFICIENCY;
		int totalMiners = 0;		
		List<string> kees = new List<string>(this.MINERS.Keys);
		foreach (string key in kees){
			totalMiners += this.MINERS[key];
		}
		double miningPowerConsumption = totalMiners * 5 * POWER_COEFFICIENT;
		double refineryPowerConsumption = this.REFINERIES * 10 * POWER_COEFFICIENT;
		double factoryPowerConsumption = this.FACTORIES * 10 * POWER_COEFFICIENT;
		double cryptoPowerConsumption = this.CRYPTO_NODES * 20 * POWER_COEFFICIENT;
		return miningPowerConsumption + refineryPowerConsumption + factoryPowerConsumption + cryptoPowerConsumption;
	}

	void decreaseFactories(){
		this.FACTORIES = this.FACTORIES - 1;
	}

	void decreaseRefineries(){
		this.REFINERIES = this.REFINERIES - 1;
	}

	void buildGenerator(){
		this.GENERATORS = this.GENERATORS + 1;
	}

}
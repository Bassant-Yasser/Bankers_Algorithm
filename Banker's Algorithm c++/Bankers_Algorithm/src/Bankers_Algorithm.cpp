
#include <iostream>
#include <vector>
using namespace std;

/* n is number of processes, m is number of resources types */
int n, m;

vector<int> available;

/* vector to store available vector before modifying it */
vector<int> available2;

vector<vector<int>> need;
vector<vector<int>> allocation;

bool is_safe(vector<int> &safe);
void  resource_request(vector<int> request, int process);

int main() {
	int num, need_safe_or_request, requested_process;

	while(1)
	{
	    vector<vector<int>> max;

		cout << endl <<  "Enter number of processes and number of resources:  ";
		cin >> n >> m;

		/* asking user for allocation matrix */
		cout << endl << "Enter allocation matrix" << endl;
		for (int i = 0; i < n; i++)
		{
			vector<int> row;
			cout << "P" << i << "  ";
			for (int j = 0; j < m; j++) {
				cin >> num;
				row.push_back(num);
			}
			allocation.push_back(row);
		}

		/* asking user for max matrix */
		cout << endl << "Enter max matrix" << endl;
		for (int i = 0; i < n; i++)
		{
			vector<int> row;
			cout << "P" << i << "  ";
			for (int j = 0; j < m; j++) {
				cin >> num;
				row.push_back(num);
			}
			max.push_back(row);
		}

		/* asking user for available resources vector */
		cout << endl << "Enter available resources vector" << endl;
		for(int i = 0; i < m; i++)
		{
			cin >> num;
			available.push_back(num);
			available2.push_back(num);
		}

		/* calculating need matrix */
		for (int i = 0; i < n; i++)
		{
			vector<int> row;
			for (int j = 0; j < m; j++)
			{
				num = max[i][j] - allocation[i][j];
				row.push_back(num);
			}
			need.push_back(row);
		}

		/* printing need matrix */
		cout << endl << "Need Matrix:" << endl;
		cout << "   ";
		for (int i = 0; i < m; i++)
		{
			cout << "R" << i << " ";
		}
		cout << endl;
		for (int i = 0; i < n; i++)
		{
			cout << "P" << i << " ";
			for (int j = 0; j < m; j++)
			{
				cout << need[i][j] << "  ";
			}
			cout << endl;

		}

		/* The user can enquire if the system is in a safe state */
		cout << endl << "Safe State ? Yes:1 , No:0" << endl;
		cin >> need_safe_or_request;
		if(need_safe_or_request)
		{
			vector<int> safe_vector;
			need_safe_or_request = is_safe(safe_vector);
			if(need_safe_or_request == 0)
			{
				cout << "Not Safe" << endl;
			}
			else
			{
				cout << "Yes, Safe state <";
				for(int i = 0; i < n; i++)
				{
					cout << "P" << safe_vector[i];
					if(i != n - 1)
					{
						cout << ",";
					}
				}
				cout << ">" << endl;
			}
		}

		/* The user can enquire if a certain immediate request by one of the processes can be granted  */
		cout << endl << "Request ? Yes:1 , No:0"<<endl;
		cin >> need_safe_or_request;
		if(need_safe_or_request)
		{
			cout << "Enter number of process:  ";
			cin >> requested_process;
			vector<int> request;
			cout << endl << "Enter requested resources:  ";
			for(int i = 0; i < m; i++)
			{
				cin >> num;
				request.push_back(num);
			}
			resource_request(request,  requested_process);
			cout << endl;
		}

		/* Removing all elements of vectors before starting new loop */
		available.clear();
		available2.clear();
		allocation.clear();
		max.clear();
		need.clear();
		allocation.clear();

		cout << "Do you want to repeat ? 1: Yes, 0: No" << endl;
		cin >> num;
		if(num == 0)
			break;
	}


}

void  resource_request(vector<int> request, int process)
{
	bool requesting = true;

	/* Step 1: If request <= need go to step 2. Otherwise, raise error condition,
		since process has exceeded its maximum claim */
	for(int i = 0; i < m; i++)
	{
		if(request[i] > need[process][i])
		{
			requesting = false;
			break;
		}
	}

	/* Step 2:  If request <= available, go to step 3. Otherwise Process must wait,
	since resources are not available */
	if(requesting)
	{
		for(int i = 0; i < m; i++)
		{
			if(request[i] > available[i])
			{
				requesting = false;
				break;
			}
		}
	}
	else
	{
		cout << "Process has exceeded its maximum claim" << endl;
		return;
	}

	/* Step 3:  Pretend to allocate requested resources to Pi by modifying the state
	 * If safe: resources are allocated to process
	 * If unsafe: process must wait and the old resource-allocation state is restored
	 */
	if(requesting)
	{
		for(int i = 0; i < m; i++)
		{
			available2[i] -= request[i];
			allocation[process][i] += request[i];
			need[process][i] -= request[i];
		}
		vector<int> safe_vector;
		int safe = is_safe(safe_vector);
		if(!safe)
		{
			for(int i = 0; i < m; i++)
			{
				available[i] += request[i];
				allocation[process][i] -= request[i];
				need[process][i] += request[i];
			}
			cout << "Request can not be granted as no safe state";
		}
		else
		{
			cout << "Yes request can be granted with safe state, Safe state <P" << process << "req,";
			for(int i = 0; i < n; i++)
			{
				cout << "P" <<  safe_vector[i];
				if(i != n - 1)
				{
					cout << ",";
				}
			}
			cout << ">" << endl;
		}
	}
	else
		cout << "Process must wait resources not available" << endl;
}

bool is_safe(vector<int> &safe)
{
	bool found = true;
	vector<int> finish(n, 0);
	for(int i = 0; i < m; i++)
	{
		available[i] = available2[i];
	}

	/* If Finish [i] == true for all i, then the system is in a safe state */
	while(found)
	{
		found = false;
		/* Find an i such that both:
		 * a) Finish [i] = false
		 * b) Need <= Available
		 */
		for(int i = 0; i < n; i++)
		{
			if(finish[i] == 0)
			{
				found = true;

				for(int j = 0; j < m; j++)
				{
					if(need[i][j] > available[j])
					{
						found = false;
					}
				}
				if(found)
				{
					safe.push_back(i);
					for(int j = 0; j < m; j++)
					{
						available[j] = available[j] + allocation[i][j];
					}
					finish[i] = 1;
				}
			}
		}
	}

	int x = safe.size();
	return x;
}

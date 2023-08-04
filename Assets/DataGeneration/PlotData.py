import pandas as pd
from matplotlib import pyplot as plt
import glob

def plot_graph_csv_files(folder_path):
    # Use glob to get a list of all CSV files in the folder
    file_list = glob.glob(folder_path + '/*.csv')

    count = 0
    # Loop through each CSV file and plot its data
    for file_path in file_list:
        # Create a new figure for each plot
        plt.figure()

        # Read the CSV data into a pandas DataFrame
        df = pd.read_csv(file_path)

        S = df['Susceptible']
        E = df['Exposed']
        I = df['Infected']
        T = df['Time']

        # Plotting the lines
        plt.plot(T, S, label='At time Susceptible', color='blue')
        plt.plot(T, E, label='Total Exposed', color='yellow')
        plt.plot(T, I, label='At time Infected', color='red')

        # Adding labels and title
        plt.xlabel('Time')
        plt.ylabel('Agents')
        plt.title('Total Exposed as time goes on')

        # Adding a legend to distinguish the lines
        plt.legend()

        # Save the plot to a file
        plt.savefig("Plots/Graphs/plot" + str(count) + ".png")
        count = count + 1
    
def plot_bar_csv_files(folder_path):
    # Use glob to get a list of all CSV files in the folder
    file_list = glob.glob(folder_path + '/*.csv')

    count = 0
    # Loop through each CSV file and plot its data
    for file_path in file_list:
        # Create a new figure for each plot
        plt.figure()

        df = pd.read_csv(file_path)

        # Group the data by "Area" and sum the hits for each area
        area_hit_totals = df.groupby('Area')['Hit'].sum()

        # Print the total hits for each area
        for area, total_hits in area_hit_totals.items():
            #print(f"Area: {area}, Total Hits: {total_hits}")
                plt.bar(area, total_hits)
                plt.xlabel('Area')
                plt.ylabel('Infections')
                plt.title(f'Total Infections')
                plt.xticks(rotation=90)
                plt.tight_layout()
        
        # Save the plot to a file
        plt.savefig("Plots/Bars/bar" + str(count) + ".png")
        count = count + 1


plot_graph_csv_files('Datasets/Graphs')
plot_bar_csv_files('Datasets/Bars')




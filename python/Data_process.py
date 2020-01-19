import csv
import os
import glob

# engagement -- row[2]
# stress -- row[13]
# relaxation -- row[18]
# interest -- row[23]

path = "C:\\MITHack\\YOUtopia\\EEG\\*.*"
target_engagement = "C:\\MITHack\\YOUtopia\\Unity\\Assets\\Data\\engagement.csv"
# target_engagement_preview = "./engagement_preview.csv"
target_stress = "C:\\MITHack\\YOUtopia\\Unity\\Assets\\Data\\stress.csv"
# target_stress_preview = "./stress_preview.csv"
target_relaxation = "C:\\MITHack\\YOUtopia\\Unity\\Assets\\Data\\relaxation.csv"
# target_relaxation_preview = "./relaxation_preview.csv"
target_interest = "C:\\MITHack\\YOUtopia\\Unity\\Assets\\Data\\interest.csv"
# target_interest_preview = "./interest_preview.csv"

#file =[]
file_name = ""

line_timestamp = []
line_engagement = []
line_stress = []
line_relaxation = []
line_interest = []


first_line = True
second_line = False
third_line = False

timestamp = 0

file = glob.glob(path)
file_name = max(file, key = os.path.getctime)


print(file_name)


#read and mapping data

with open(file_name, 'r', encoding='UTF-8') as f:
    reader = csv.reader(f, delimiter=',')

    for row in reader:
    #    print(row)
       
        if first_line:
        #    line_timestamp.append(row[0])
        #    line_engagement.append(row[4])
            first_line = False
            second_line = True
            continue
        
        if second_line:
            line_timestamp.append(row[0])
            line_engagement.append(row[2])
            line_stress.append(row[13])
            line_relaxation.append(row[18])
            line_interest.append(row[23])
            second_line = False
            third_line = True
            continue
        
        
        if third_line:
            line_timestamp.append(0.1)
            line_engagement.append(row[2])
            line_stress.append(row[13])
            line_relaxation.append(row[18])
            line_interest.append(row[23])
            third_line = False
            continue
        
        timestamp += 10


        line_timestamp.append(timestamp)
#        line_engagement.append(float(row[2])+0.01)          # mapping to texture.R [0.01, 1] 
#        line_interest.append(float(row[23])+1)              # mapping to material.Density [1, 2]

        # line_stress.append(float(row[13]) + 1)          # mapping to material.Density [1, 2]
        # line_relaxation.append(float(row[18])*0.8 -0.4)        # mapping to texture.N [-0.4, 0.4]
        # line_interest.append(float(row[23])*2 -1)        # mapping to material.Preview [-1, 1.0]
        # line_engagement.append(float(row[2])*4.5 + 1.5)        # mapping to texture.N [1.5, 6]
    
        line_stress.append(float(row[13]))        
        line_relaxation.append(float(row[18]))    
        line_interest.append(float(row[23]))       
        line_engagement.append(float(row[2]))       
    

    


#output_range = [0, 8, 10]  
def write_file(name,value):
    with open(name, "w", encoding= "UTF-8", newline="") as f:
        writer = csv.writer(f)

        for index in range(len(line_timestamp)-1):
        #for index in output_range:
            writer.writerow([line_timestamp[index], value[index]])


write_file(target_engagement,line_engagement)

write_file(target_interest, line_interest)

write_file(target_relaxation, line_relaxation)

write_file(target_stress, line_stress)


print(line_timestamp)
print(line_interest)


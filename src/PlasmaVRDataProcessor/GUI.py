import PySimpleGUI as sg
import ParticleProcessor
import ScalarProcessor
import VectorProcessor
import os

sg.theme('DarkAmber')   # Add a touch of color

debug_print = sg.Print

particle_keys = []
pfolder = ""

scalar_keys = []
sfolder = ""

vector_keys = []
vfolder = ""

pc1 = sg.Column([[sg.Text("Group Key to Process:")],[ sg.Listbox(particle_keys, key='-PGROUP-', select_mode='single', size=(20, 6))]])
pc2 = sg.Column([[sg.Text("Results Folder:")],[sg.InputText(key='-PRESULTS-')], [sg.Push(),sg.Button(key='-PEXECUTE-', button_text="Process Data")]])
particle_layout = [
    [sg.Column([[sg.Text('Dataset Folder:'),sg.Push(), sg.In(enable_events=True, key='-PFOLDER-'), sg.FolderBrowse()]])],
    [pc1, pc2],
    [sg.Text(key="-PSTATE-")]
]

sc1 = sg.Column([[sg.Text("Scalar Key to Process:")],[ sg.Listbox(particle_keys, key='-SGROUP-', select_mode='single', size=(20, 6))]])
sc2 = sg.Column([[sg.Text("Results Folder:")],[sg.InputText(key='-SRESULTS-')],[sg.Text("Isovalue:")],[sg.InputText(key='-ISOVALUE-')],[sg.Checkbox("Output Raws", key='-SRAWS-', enable_events=True)], [sg.Push(),sg.Button(key='-SEXECUTE-', button_text="Process Data")]])
scalar_layout = [
    [sg.Column([[sg.Text('Dataset Folder:'), sg.Push(),sg.In(enable_events=True, key='-SFOLDER-'), sg.FolderBrowse()]])],
    [sc1, sc2],
    [sg.Text(key="-SSTATE-")]
]

vector_layout = [
    [sg.Text('Dataset Folder:')],
    [sg.In(enable_events=True, key='-VFOLDER-', expand_x=True), sg.FolderBrowse()],
    [sg.Text("Results Folder:")],
    [sg.InputText(key='-VRESULTS-', expand_x=True)],
    [sg.Push(), sg.Button(key='-VEXECUTE-', button_text="Process Data")],
    [sg.Text(key="-VSTATE-")]
]

tab_group = sg.TabGroup([[
        sg.Tab('Particle Editor', particle_layout),
        sg.Tab('Scalar Field Editor', scalar_layout),
        sg.Tab('Vector Field Editor', vector_layout)
    ]])

layout = [
    [tab_group],
    [sg.Checkbox("Show Debug Information", key="-DEBUG-", enable_events=True),sg.Push(), sg.Button(key='Exit', button_text="Exit")]
]

window = sg.Window('PlasmaVR Data Processor', layout , resizable=False, icon="PlasmaVRLogo.ico")

while True:
    event, values = window.read()
    if event in (sg.WIN_CLOSED, 'Exit'):
        break

    #PARTICLE EVENTS
    if values['-PFOLDER-'] != pfolder:
        #changed folder
        pfolder = values['-PFOLDER-']
        particle_keys = ParticleProcessor.getKeys(pfolder)
        if particle_keys == False:
            window['-PSTATE-'].update("Invalid dataset folder")
        else:
            window['-PGROUP-'].update(values=particle_keys)
    if event == '-DEBUG-':
        if values['-DEBUG-']:
            ParticleProcessor.print = debug_print
    if event == '-PEXECUTE-':
        pfolder = values['-PFOLDER-']
        presults = values['-PRESULTS-']
        pgroup = values['-PGROUP-'][0]

        if pfolder == "":
            window['-PSTATE-'].update("NO FOLDER")
            continue

        elif pgroup == "":
            window['-PSTATE-'].update("NO GROUP")
            continue
        else:
            window['-PSTATE-'].update("")

        if presults == "":
            presults = "part_results"

        # Check if folder exists
        if os.path.isdir(presults):
            sg.Popup('Folder already exists.')
        else:

            res = ParticleProcessor.particleSequence(pfolder, presults, pgroup)

            if res:
                window['-PSTATE-'].update("Success, results available in folder " + presults)


    #SCALAR EVENTS
    if values['-SFOLDER-'] != sfolder:
        #changed folder
        sfolder = values['-SFOLDER-']
        scalar_keys = ScalarProcessor.getKeys(sfolder)
        print(scalar_keys)
        if scalar_keys == False:
            window['-SSTATE-'].update("Invalid dataset folder")
        else:
            window['-SGROUP-'].update(values=scalar_keys)

    if event == '-DEBUG-':
        if values['-DEBUG-']:
            ScalarProcessor.print = debug_print

    if event == '-SEXECUTE-':
        sfolder = values['-SFOLDER-']
        sresults = values['-SRESULTS-']
        sgroup = values['-SGROUP-'][0]
        isovalue = values['-ISOVALUE-']

        if sfolder == "":
            window['-SSTATE-'].update("NO FOLDER")
            continue

        elif sgroup == "":
            window['-SSTATE-'].update("NO GROUP")
            continue
        elif isovalue == "":
            window['-SSTATE-'].update("NO ISOVALUE")
            continue
        else:
            window['-SSTATE-'].update("")

        if sresults == "":
            sresults = "scalar_results"

        # Check if folder exists
        if os.path.isdir(sresults):
            sg.Popup('Folder already exists.')
        else:

            res = ScalarProcessor.isosurfaceSequence(sfolder, sgroup, float(isovalue), sresults, raws=values['-SRAWS-'])

            if res:
                window['-SSTATE-'].update("Success, results available in folder " + sresults)


    # VECTOR EVENTS
    if event == '-DEBUG-':
        if values['-DEBUG-']:
            VectorProcessor.print = debug_print

    if event == '-VEXECUTE-':
        vfolder = values['-VFOLDER-']
        vresults = values['-VRESULTS-']


        if vfolder == "":
            window['-VSTATE-'].update("NO FOLDER")
            continue
        else:
            window['-VSTATE-'].update("")

        if vresults == "":
            vresults = "vector_results"

        #Check if folder exists
        if os.path.isdir(vresults):
            sg.Popup('Folder already exists.')
        else:
            res = VectorProcessor.writeSequence(vfolder, vresults)

            if res:
                window['-VSTATE-'].update("Success, results available in folder " + vresults)
            else:
                window['-VSTATE-'].update("Failure")

window.close()

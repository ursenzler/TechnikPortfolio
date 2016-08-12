document.title = "{{radar_title}}";

var radar_arcs = [
    {'r':100,'name':'Beherrschen'},
    {'r':200,'name':'Experimentieren'},
    {'r':300,'name':'Beobachten'},
    {'r':400, 'name':'Nicht verwenden'}];

var h = 900;
var w = 1500;

var radar_data = [
    { "quadrant": "Libraries and Frameworks",
        "left" : 45,
        "top" : 18,
        "color" : "#8FA227",
        "items" : {{top_left_data}}
    },
    { "quadrant": "Concepts, Methods and Processes",
        "left": w-320,
        "top" : 18,
        "color" : "#587486",
        "items" : {{top_right_data}}
    },
    { "quadrant": "Services, Products, Tools",
        "left" :45,
         "top" : (h/2 + 18),
        "color" : "#DC6F1D",
        "items" : {{bottom_left_data}}
    },
    { "quadrant": "Theme",
        "color" : "#B70062",
        "left"  : (w-320),
        "top" :   (h/2 + 18),
        "items" : {{bottom_right_data}}
    }
];
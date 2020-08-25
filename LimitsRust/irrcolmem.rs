fn main() {
    unsafe {
        let layout = std::alloc::Layout::new::<(u16, bool, String)>(); // Tuple specifies size only
        let ptr = std::alloc::alloc(layout);
        *(ptr as *mut u16) = 3149;
        assert_eq!(*(ptr as *mut u16), 3149);
        *(ptr.offset(1) as *mut String) = String::from("hey");
        assert_eq!(*(ptr.offset(1) as *mut String), String::from("hey"));
        // *(ptr.offset(2) as *mut bool) = false; // Erases the string
        assert_eq!(*(ptr.offset(2) as *mut bool), false);
        assert_eq!(*(ptr.offset(1) as *mut String), String::from("hey")); // Error
        assert_eq!(*(ptr as *mut u16), 3149); // Error
        // *(ptr.offset(2) as *mut String) = String::from("hey hey hey hey"); // Segmentation fault
    }
}
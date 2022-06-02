if (typeof (ninotree) === 'undefined') {
    var ninotree = {
        options: {
            rightArrow: 'fa-chevron-right',
            downArrow: 'fa-chevron-down'
        },
        single: {
            fields: {
                id: '',
                html: ''
            },
            methods: {
                makeForm: function (data) {
                    ninotree.single.fields.html = `<form id="${ninotree.single.fields.id}Form">`;
                    ninotree.single.fields.html += ninotree.single.methods.makeHTML(data);
                    ninotree.single.fields.html += '</form>';
                    return ninotree.single.fields.html;
                },
                makeHTML: function (data) {
                    if (data && data.length > 0) {
                        ninotree.single.fields.html += '<ul>';
                        data.forEach(node => {
                            var hasChild = node.childs && node.childs.length > 0;
                            if (hasChild) {
                                ninotree.single.fields.html += ninotree.single.methods.getParentNode(node);
                                ninotree.single.methods.makeHTML(node.childs);
                                ninotree.single.fields.html += '</li>';
                            }
                            else {
                                ninotree.single.fields.html += ninotree.single.methods.getChildNode(node);
                            }
                        });
                        ninotree.single.fields.html += '</ul>';
                    }
                    return ninotree.single.fields.html;
                },
                registerCarets: function () {
                    var carets = document.getElementsByClassName('caret');
                    for (caret of carets) {
                        caret.addEventListener('click', function () {
                            this.parentElement.querySelector('ul').classList.toggle('collapsed');
                            this.classList.toggle(ninotree.options.rightArrow);
                            this.classList.toggle(ninotree.options.downArrow);
                        });
                    }
                },
                getParentNode: function (node) {
                    return `<li><i class="fas ${ninotree.options.downArrow}  caret"></i><input class="custom-control-input" type="radio" ${node.selected ? 'checked' : ''} name="${ninotree.single.fields.id}" id="${node.id}" value="${node.id}"><label class="custom-control-label" for="${node.id}">${node.name}</label>`;
                },
                getChildNode: function (node) {
                    return `<li><input class="custom-control-input" type="radio" ${node.selected ? 'checked' : ''} name="${ninotree.single.fields.id}" id="${node.id}" value="${node.id}"><label class="custom-control-label" for="${node.id}">${node.name}</label></li>`;
                }
            },
            init: function (id, data) {
                ninotree.single.fields.id = id;
                var htmlNode = document.getElementById(id);
                if (!htmlNode) {
                    console.error('tree does not exist');
                    return;
                }
                htmlNode.classList.add('ninotree');
                htmlNode.classList.add('custom-control');
                htmlNode.classList.add('custom-radio');
                htmlNode.innerHTML = ninotree.single.methods.makeForm(data);
                ninotree.single.methods.registerCarets();
                ninotree.single.collapse();
            },
            collapse:function(){
                var uls = document.getElementById(ninotree.single.fields.id).querySelector('ul').querySelectorAll('ul');
                uls.forEach(ul =>{
                    ul.classList.add('collapsed');
                    var i = ul.parentElement.querySelector('i');
                    i.classList.remove(ninotree.options.downArrow);
                    i.classList.add(ninotree.options.rightArrow);
                });
            },
            expand:function(){
                var uls = document.getElementById(ninotree.single.fields.id).querySelector('ul').querySelectorAll('ul');
                uls.forEach(ul =>{
                    ul.classList.remove('collapsed');
                    var i = ul.parentElement.querySelector('i');
                    i.classList.remove(ninotree.options.rightArrow);
                    i.classList.add(ninotree.options.downArrow);
                });
            },
            getSelected: function () {
                var htmlNode = document.getElementById(ninotree.single.fields.id + 'Form');
                var form = new FormData(htmlNode);
                var result = form.get(ninotree.single.fields.id);
                console.log(result);
                return result;
            }
        },
        multiple: {}
    }
}
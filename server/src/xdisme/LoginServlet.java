package xdisme;

import com.alibaba.fastjson.JSON;
import com.alibaba.fastjson.JSONObject;
import org.apache.struts2.ServletActionContext;
import org.hibernate.Session;
import org.hibernate.SessionFactory;
import org.hibernate.Transaction;
import org.hibernate.cfg.AnnotationConfiguration;
import org.hibernate.cfg.Configuration;
import xdisme.model.JsonData;
import xdisme.model.User;
import xdisme.utils.ResponseJsonUtils;

import javax.servlet.*;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.io.IOException;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class LoginServlet extends HttpServlet {
    private static final SessionFactory ourSessionFactory;
    private static Session session;
    static {
        try {
            Configuration configuration = new AnnotationConfiguration();
            configuration.configure();

            ourSessionFactory = configuration.buildSessionFactory();
        } catch (Throwable ex) {
            throw new ExceptionInInitializerError(ex);
        }
    }
    public List<User> get(Map map){
        //数据库中进行查询

        Transaction t = session.beginTransaction();
        List<User> users = (List<User>)session.createQuery("select id from User user where account = ? and nickname = ?")
                .setParameter(0, map.get("UserName"))
                .setParameter(1, map.get("Password"))
                .list();
        t.commit();
        return users;
    }
    public void update(int id){
        //数据库中进行查询
        Transaction t = session.beginTransaction();
        User user = (User)session.get(User.class, id);
        user.setOnline("1");
        session.save(user);
        t.commit();
    }

    //解析请求的Json数据
    private String getRequestPostData(HttpServletRequest request) throws IOException {
        int contentLength = request.getContentLength();
        if(contentLength<0){
            return null;
        }
        byte buffer[] = new byte[contentLength];
        for (int i = 0; i < contentLength;) {
            int len = request.getInputStream().read(buffer, i, contentLength - i);
            if (len == -1) {
                break;
            }
            i += len;
        }
        return new String(buffer, "utf-8");
    }


    @Override
    protected void doGet(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
        System.out.println("get");
        doPost(req,resp);

    }

    @Override
    protected void doPost(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
//        System.out.println("post");
//        String type = req.getHeader("Content-Type");
//        System.out.println("type"+":"+type);
        String jsonStr  = null;
        try {
            jsonStr = getRequestPostData(req);
        } catch (IOException e) {
            e.printStackTrace();
        }
        session =  ourSessionFactory.openSession();



        //将接受到的json转喂JSonAbj
        Map<String, Object> data = new HashMap<String, Object>();
        Map mapType = JSON.parseObject(jsonStr,Map.class);

        for (Object obj : mapType.keySet()){
            System.out.println("key为："+obj+"值为："+mapType.get(obj));
        }

        //创建json数据对象
        JsonData jsonData = new JsonData();
        jsonData.setType("1");
        jsonData.setData("1");
        List<User> users = get(mapType);

        //用户名存在
        if(users.size() == 1){
            // todo 设置在线为 1
            System.out.println("hello");
            int id = Integer.parseInt(users.get(0)+"");
            System.out.println("id"+id);
            update(id);
            jsonData.setCode("1");
        } //用户名不存在
        else{
            jsonData.setCode("1002");
        }
        session.close();

        ResponseJsonUtils.json(resp, jsonData);
        System.out.println(data.toString());
    }
}
